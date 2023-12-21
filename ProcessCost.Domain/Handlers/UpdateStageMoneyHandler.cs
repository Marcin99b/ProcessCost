using MediatR;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Handlers;

public record UpdateStageMoneyRequest(Guid StageId, Money Money) : IRequest<UpdateStageMoneyResponse>;

public record UpdateStageMoneyResponse;

public class UpdateStageMoneyHandler(IStagesRepository repository, IStagesEventBus eventBus)
    : IRequestHandler<UpdateStageMoneyRequest, UpdateStageMoneyResponse>
{
    public async Task<UpdateStageMoneyResponse> Handle(UpdateStageMoneyRequest request,
        CancellationToken cancellationToken)
    {
        var stage = (await repository.GetStageById(request.StageId))!;
        var oldMoney = stage.Money; //it's record so it's cloned

        stage.UpdateMoney(request.Money);

        await repository.Update(stage);
        await eventBus.Publish(new StageUpdatedMoneyEvent(oldMoney, stage));

        return new();
    }
}