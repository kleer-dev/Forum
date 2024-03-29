using MediatR;

namespace Recommendations.Application.CommandsQueries.User.Commands.Unblock;

public class UnblockUserCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }

    public UnblockUserCommand(Guid userId)
    {
        UserId = userId;
    }
}