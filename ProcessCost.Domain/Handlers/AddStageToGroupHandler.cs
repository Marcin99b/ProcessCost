using MediatR;

namespace ProcessCost.Domain.Handlers;

public record AddStageToGroupRequest(Guid GroupId, Guid StageId) : IRequest<AddStageToGroupResponse>;

public record AddStageToGroupResponse;

public class AddStageToGroupHandler(IStagesGroupsRepository stagesGroupsRepository, IStagesRepository stagesRepository)
    : IRequestHandler<AddStageToGroupRequest, AddStageToGroupResponse>
{
    public async Task<AddStageToGroupResponse> Handle(AddStageToGroupRequest request,
        CancellationToken cancellationToken)
    {
        var stage = await stagesRepository.GetStageById(request.StageId);
        if (stage == null)
        {
            throw new NullReferenceException();
        }

        var group = await stagesGroupsRepository.GetById(request.GroupId);
        group!.AddStage(stage);
        await stagesGroupsRepository.Update(group);
        return new();
    }
}