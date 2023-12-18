using ProcessCost.Database.Entities;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;
public class StagesRepository(DatabaseContext context) : IStagesRepository
{
    public Stage GetStageById(Guid stageId)
    {
        var stages = this.GetAllStagesOfUser(Guid.Empty);
        return stages.First();
    }

    public IEnumerable<Stage> GetAllStagesOfUser(Guid userId)
    {
        var result = context.Stages.Select(x =>
            new Stage(x.Name, x.Day, new(x.MoneyAmount, Enum.Parse<Currency>(x.MoneyCurrency))) { Id = x.Id, });

        return result.AsEnumerable();
    }

    public async Task Add(Stage stage)
    {
        var entity = new StageEntity() { Id = stage.Id, Day = stage.Day, Name = stage.Name, MoneyAmount = stage.Money.CalculationAmount, MoneyCurrency = stage.Money.Currency.ToString() };
        await context.Stages.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public Task Update(Stage stage)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid stageId)
    {
        throw new NotImplementedException();
    }
}
