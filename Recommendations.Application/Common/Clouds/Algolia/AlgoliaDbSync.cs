using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recommendations.Application.Interfaces;
using Recommendations.Domain;

namespace Recommendations.Application.Common.Clouds.Algolia;

public class AlgoliaDbSync
{
    private readonly IRecommendationsDbContext _context;
    private readonly IAlgoliaService _algoliaService;
    private readonly IMapper _mapper;

    public AlgoliaDbSync(IRecommendationsDbContext context,
        IAlgoliaService algoliaService, IMapper mapper)
    {
        _context = context;
        _algoliaService = algoliaService;
        _mapper = mapper;
    }

    public async Task Synchronize()
    {
        var trackedEntities = _context.ChangeTracker
            .Entries<Discussion>()
            .ToList();
        foreach (var entity in trackedEntities)
            await StartOperation(entity);
    }

    private async Task StartOperation(EntityEntry<Discussion> entityEntry)
    {
        var entity = entityEntry.Entity;
        await LoadAllEntityReferences(entity);
        var sendDto = _mapper.Map<AlgoliaDto>(entity);

        switch (entityEntry.State)
        {
            case EntityState.Added or EntityState.Modified or EntityState.Unchanged:
                await _algoliaService.AddOrUpdateRecord(sendDto);
                break;
            case EntityState.Deleted:
                await _algoliaService.DeleteRecord(entity);
                break;
        }
    }

    private async Task LoadAllEntityReferences(Discussion discussion)
    {
        await _context.Discussions.Entry(discussion)
            .Reference(r => r.Category).LoadAsync();
        await _context.Discussions.Entry(discussion)
            .Collection(r => r.Comments).LoadAsync();
        await _context.Discussions.Entry(discussion)
            .Collection(r => r.Likes).LoadAsync();
        await _context.Discussions.Entry(discussion)
            .Reference(r => r.Theme).LoadAsync();
        await _context.Discussions.Entry(discussion)
            .Collection(r => r.Tags).LoadAsync();
    }
}