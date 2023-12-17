using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record GetStagesRequest : IRequest<GetStagesResponse>;

public record GetStagesResponse(Stage[] Stages);

public class GetStagesHandler(IStagesRepository stagesRepository) : IRequestHandler<GetStagesRequest, GetStagesResponse>
{
    public async Task<GetStagesResponse> Handle(GetStagesRequest request, CancellationToken cancellationToken)
    {
        var stages = await stagesRepository.GetAllStagesOfUser(Guid.Empty);
        return new GetStagesResponse(stages.ToArray());
    }
}