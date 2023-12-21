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
        var stage = await stagesRepository.GetStageById(request.StageId);
        if (stage == null)
        {
            throw new NullReferenceException();
        }

        var group = await stagesGroupsRepository.GetById(request.GroupId);
        group!.RemoveStage(stage);
        await stagesGroupsRepository.Update(group);
        return new();
    }
}