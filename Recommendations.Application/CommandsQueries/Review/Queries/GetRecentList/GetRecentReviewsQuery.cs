using MediatR;

namespace Recommendations.Application.CommandsQueries.Review.Queries.GetRecentList;

public class GetRecentReviewsQuery : IRequest<GetAllReviewsVm>
{
    public int? Count { get; set; }

    public GetRecentReviewsQuery(int? count)
    {
        Count = count;
    }
}