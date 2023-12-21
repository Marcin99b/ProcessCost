using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.WebApi;

public class EventBusExecutor(IStagesEventBus eventBus, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scope = serviceProvider.CreateAsyncScope();
        var groupsRepository = scope.ServiceProvider.GetService<IStagesGroupsRepository>()!;
        await foreach (var stageEvent in eventBus.Subscribe(stoppingToken))
        {
            await (stageEvent switch 
            {
                StageUpdatedMoneyEvent x => this.ProcessStageUpdatedMoneyEvent(groupsRepository, x),
                _ => Task.CompletedTask,
            });
        }
    }

    private async Task ProcessStageUpdatedMoneyEvent(IStagesGroupsRepository groupsRepository,
        StageUpdatedMoneyEvent updatedMoneyEvent)
    {
        var moneyDifference = updatedMoneyEvent.UpdatedStage.Money.CalculationAmount -
            updatedMoneyEvent.OldMoney.CalculationAmount;
        var groups = groupsRepository.GetGroupsByStageId(updatedMoneyEvent.UpdatedStage.Id);
        await foreach (var group in groups)
        {
            //todo check currency change
            var newMoney = new Money(group.Money.CalculationAmount + moneyDifference, group.Money.Currency);
            group.UpdateMoney(newMoney);
            await groupsRepository.Update(group);
        }
    }
}