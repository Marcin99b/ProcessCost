using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record AddStageRequest(string Name, int Day, decimal Amount, string Currency) : IRequest<AddStageResponse>;

public record AddStageResponse;

public class AddStageHandler(IStagesRepository stagesRepository) : IRequestHandler<AddStageRequest, AddStageResponse>
{
    public async Task<AddStageResponse> Handle(AddStageRequest request, CancellationToken cancellationToken)
    {
        var stage = new Stage(request.Name, request.Day, new (request.Amount, Enum.Parse<Currency>(request.Currency)));
        await stagesRepository.Add(stage);
        return new();
    }
}