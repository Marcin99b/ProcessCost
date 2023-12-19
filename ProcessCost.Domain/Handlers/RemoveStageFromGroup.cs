using MediatR;

namespace ProcessCost.Domain.Handlers;

public record RemoveStageFromGroupRequest(Guid GroupId, Guid StageId) : IRequest<RemoveStageFromGroupResponse>;

public record RemoveStageFromGroupResponse;

public class RemoveStageFromGroupHandler(
    IStagesGroupsRepository stagesGroupsRepository,
    IStagesRepository stagesRepository)
    : IRequestHandler<RemoveStageFromGroupRequest, RemoveStageFromGroupResponse>
{
    public async Task<RemoveStageFromGroupResponse> Handle(RemoveStageFromGroupRequest request,
        CancellationToken cancellationToken)
    {
        var stage = stagesRepository.GetStageById(request.StageId);
        var group = stagesGroupsRepository.GetById(request.GroupId);
        group.RemoveStage(stage);
        await stagesGroupsRepository.Update(group);
        return new();
    }
}