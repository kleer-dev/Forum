﻿using MediatR;
using Recommendations.Application.CommandsQueries.User.Queries.Get;
using Recommendations.Application.Interfaces;

namespace Recommendations.Application.CommandsQueries.User.Commands.ChangeAvatar;

public class ChangeAvatarCommandHandler : IRequestHandler<ChangeAvatarCommand, Unit>
{
    private readonly IRecommendationsDbContext _context;
    private readonly IFirebaseService _firebaseService;
    private readonly IMediator _mediator;

    public ChangeAvatarCommandHandler(IRecommendationsDbContext context,
        IFirebaseService firebaseService, IMediator mediator)
    {
        _context = context;
        _firebaseService = firebaseService;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ChangeAvatarCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await GetUser(request.UserId);
        
        if (user.FolderName != string.Empty)
            await _firebaseService.DeleteFolder(user.FolderName!);

        if (request.Image is not null)
        {
            var folderName = Guid.NewGuid().ToString();
            user.ImageUrl = (await _firebaseService.UploadFile(request.Image, folderName)).Url;
            user.FolderName = folderName;
        }
        else
        {
            user.ImageUrl = null;
            user.FolderName = null;
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
    
    private async Task<Domain.User> GetUser(Guid userId)
    {
        var getUserQuery = new GetUserQuery(userId);
        return await _mediator.Send(getUserQuery);
    }
}