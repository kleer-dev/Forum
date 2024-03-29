using MediatR;

namespace Recommendations.Application.CommandsQueries.User.Commands.Delete;

public class DeleteUserCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }

    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}