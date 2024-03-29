using MediatR;

namespace Recommendations.Application.CommandsQueries.Review.Queries.Search;

public class SearchReviewsQuery : IRequest<GetAllReviewsVm>
{
    public string SearchQuery { get; set; }

    public SearchReviewsQuery(string searchQuery)
    {
        SearchQuery = searchQuery;
    }
}