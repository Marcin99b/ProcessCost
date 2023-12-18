using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record CreateStageGroupRequest(string Name) : IRequest<CreateStageGroupResponse>;

public record CreateStageGroupResponse;

public class CreateStageGroupHandler(IStagesGroupsRepository stagesGroupsRepository)
    : IRequestHandler<CreateStageGroupRequest, CreateStageGroupResponse>
{
    public async Task<CreateStageGroupResponse> Handle(CreateStageGroupRequest request, CancellationToken cancellationToken)
    {
        var group = new StageGroup(request.Name);
        await stagesGroupsRepository.Create(group);
        return new ();
    }
}