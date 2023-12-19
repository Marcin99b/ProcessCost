using ProcessCost.Database.Entities;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;

//todo performance tests
public class StagesRepository(DatabaseContext context) : IStagesRepository
{
    public async Task<Stage?> GetStageById(Guid stageId)
    {
        var entity = await context.Stages.FindAsync(stageId);
        return entity == null ? null : this.Map(entity);
    }

    public IEnumerable<Stage> GetAllStagesOfUser(Guid userId)
    {
        return context.Stages.Select(this.Map).AsEnumerable();
    }

    public async Task Add(Stage stage)
    {
        var entity = this.Map(stage);
        await context.Stages.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task Update(Stage stage)
    {
        var found = await context.Stages.FindAsync(stage.Id);
        if (found == null)
        {
            throw new NullReferenceException();
        }
        context.Stages.Entry(found).CurrentValues.SetValues(this.Map(stage));
        context.Stages.Update(found);
        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid stageId)
    {
        var found = await context.Stages.FindAsync(stageId);
        context.Stages.Remove(found!);
        await context.SaveChangesAsync();
    }

    private Stage Map(StageEntity entity)
    {
        return new(
            entity.Name,
            entity.Day,
            new(entity.MoneyAmount, Enum.Parse<Currency>(entity.MoneyCurrency)))
        {
            Id = entity.Id,
        };
    }

    private StageEntity Map(Stage stage)
    {
        return new()
        {
            Id = stage.Id,
            Day = stage.Day,
            Name = stage.Name,
            MoneyAmount = stage.Money.CalculationAmount,
            MoneyCurrency = stage.Money.Currency.ToString(),
        };
    }
}