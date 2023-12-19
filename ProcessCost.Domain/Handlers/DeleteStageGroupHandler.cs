using MediatR;

namespace ProcessCost.Domain.Handlers;

public record DeleteStageGroupRequest(Guid GroupId) : IRequest<DeleteStageGroupResponse>;

public record DeleteStageGroupResponse;

public class DeleteStageGroupHandler(IStagesGroupsRepository stagesGroupsRepository)
    : IRequestHandler<DeleteStageGroupRequest, DeleteStageGroupResponse>
{
    public async Task<DeleteStageGroupResponse> Handle(DeleteStageGroupRequest request,
        CancellationToken cancellationToken)
    {
        await stagesGroupsRepository.Delete(request.GroupId);
        return new();
    }
}