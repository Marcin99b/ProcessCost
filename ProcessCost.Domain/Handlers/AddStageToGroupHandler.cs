using MediatR;

namespace ProcessCost.Domain.Handlers;

public record AddStageToGroupRequest : IRequest<AddStageToGroupResponse>;

public record AddStageToGroupResponse;

public class AddStageToGroupHandler : IRequestHandler<AddStageToGroupRequest, AddStageToGroupResponse>
{
    public Task<AddStageToGroupResponse> Handle(AddStageToGroupRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}