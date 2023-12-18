using MediatR;

namespace ProcessCost.Domain.Handlers;

public record CreateStageGroupRequest : IRequest<CreateStageGroupResponse>;

public record CreateStageGroupResponse;

public class CreateStageGroupHandler : IRequestHandler<CreateStageGroupRequest, CreateStageGroupResponse>
{
    public Task<CreateStageGroupResponse> Handle(CreateStageGroupRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}