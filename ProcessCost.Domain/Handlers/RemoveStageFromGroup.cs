using MediatR;

namespace ProcessCost.Domain.Handlers;

public record RemoveStageFromGroupRequest : IRequest<RemoveStageFromGroupResponse>;

public record RemoveStageFromGroupResponse;

public class RemoveStageFromGroupHandler : IRequestHandler<RemoveStageFromGroupRequest, RemoveStageFromGroupResponse>
{
    public Task<RemoveStageFromGroupResponse> Handle(RemoveStageFromGroupRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}