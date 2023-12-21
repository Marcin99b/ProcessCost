using MediatR;

namespace ProcessCost.Domain.Handlers;

public record DeleteStageRequest(Guid StageId) : IRequest<DeleteStageResponse>;

public record DeleteStageResponse;

public class DeleteStageHandler(IStagesRepository stagesRepository) : IRequestHandler<DeleteStageRequest, DeleteStageResponse>
{
    public async Task<DeleteStageResponse> Handle(DeleteStageRequest request, CancellationToken cancellationToken)
    {
        await stagesRepository.Delete(request.StageId);
        return new();
    }
}