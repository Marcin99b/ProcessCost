using MediatR;

namespace ProcessCost.Domain.Handlers;

public record AddStageToGroupRequest(Guid GroupId, Guid StageId) : IRequest<AddStageToGroupResponse>;

public record AddStageToGroupResponse;

public class AddStageToGroupHandler(IStagesGroupsRepository stagesGroupsRepository, IStagesRepository stagesRepository)
    : IRequestHandler<AddStageToGroupRequest, AddStageToGroupResponse>
{
    public Task<AddStageToGroupResponse> Handle(AddStageToGroupRequest request, CancellationToken cancellationToken)
    {
        var stage = stagesRepository.GetStageById(request.StageId);
        var group = stagesGroupsRepository.
    }
}