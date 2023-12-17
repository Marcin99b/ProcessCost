using ProcessCost.Database.Entities;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;
public class StagesRepository : IStagesRepository
{
    private readonly StageEntity[] _stages = {
        new () {Id = Guid.NewGuid(), Day = 01, MoneyAmount = 10_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 05, MoneyAmount = 10_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 10, MoneyAmount = 100_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 12, MoneyAmount = -80_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 12, MoneyAmount = 50_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 15, MoneyAmount = 200_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 16, MoneyAmount = -30_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 19, MoneyAmount = -5_00, MoneyCurrency = "PLN", Name = "A",},
        new () {Id = Guid.NewGuid(), Day = 21, MoneyAmount = 10_00, MoneyCurrency = "PLN", Name = "A",},
    };

    public async Task<Stage> GetStageById(Guid stageId)
    {
        var stages = await this.GetAllStagesOfUser(Guid.Empty);
        return stages.First();
    }

    public async Task<IEnumerable<Stage>> GetAllStagesOfUser(Guid userId)
    {
        await Task.CompletedTask;

        var result = this._stages.Select(x =>
            new Stage(x.Name, x.Day, new(x.MoneyAmount, Enum.Parse<Currency>(x.MoneyCurrency))) { Id = x.Id, });
        return result;
    }
}
