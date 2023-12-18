using ProcessCost.Database.Entities;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;

public class StagesGroupsRepository(DatabaseContext context) : IStagesGroupsRepository
{
    public async Task Add(StageGroup group)
    {
        //todo refactor
        var groupEntity = new StageGroupEntity
        {
            Id = group.Id, Name = group.Name, MoneyAmount = group.Money.CalculationAmount,
            MoneyCurrency = group.Money.Currency.ToString(),
        };

        var registeredStagesInCurrentGroup =
            context.StagesGropusReferences.Where(x => x.StageGroupId == groupEntity.Id);

        foreach (var groupInDatabase in registeredStagesInCurrentGroup)
        {
            if (group.StagesIds.All(x => x != groupInDatabase.StageId))
            {
                context.StagesGropusReferences.Remove(groupInDatabase);
            }
        }

        foreach (var stageId in group.StagesIds)
        {
            if (!registeredStagesInCurrentGroup.Any(x => x.StageId == stageId))
            {
                await context.StagesGropusReferences.AddAsync(new()
                    { Id = Guid.NewGuid(), StageGroupId = group.Id, StageId = stageId, });
            }
        }

        await context.StagesGropus.AddAsync(new()
        {
            Id = group.Id,
            Name = group.Name,
            MoneyAmount = group.Money.CalculationAmount,
            MoneyCurrency = group.Money.Currency.ToString(),
        });

        await context.SaveChangesAsync();
    }

    public async Task Update(StageGroup group)
    {
        //todo refactor
        var groupEntity = new StageGroupEntity
        {
            Id = group.Id,
            Name = group.Name,
            MoneyAmount = group.Money.CalculationAmount,
            MoneyCurrency = group.Money.Currency.ToString(),
        };

        var registeredStagesInCurrentGroup =
            context.StagesGropusReferences.Where(x => x.StageGroupId == groupEntity.Id);

        foreach (var groupInDatabase in registeredStagesInCurrentGroup)
        {
            if (group.StagesIds.All(x => x != groupInDatabase.StageId))
            {
                context.StagesGropusReferences.Remove(groupInDatabase);
            }
        }

        foreach (var stageId in group.StagesIds)
        {
            if (!registeredStagesInCurrentGroup.Any(x => x.StageId == stageId))
            {
                await context.StagesGropusReferences.AddAsync(new()
                    { Id = Guid.NewGuid(), StageGroupId = group.Id, StageId = stageId, });
            }
        }

        context.StagesGropus.Update(new()
        {
            Id = group.Id,
            Name = group.Name,
            MoneyAmount = group.Money.CalculationAmount,
            MoneyCurrency = group.Money.Currency.ToString(),
        });

        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid groupId)
    {
        var refs = context.StagesGropusReferences.Where(x => x.StageGroupId == groupId);
        context.StagesGropusReferences.RemoveRange(refs);

        var group = context.StagesGropus.First(x => x.Id == groupId);
        context.StagesGropus.Remove(group);

        await context.SaveChangesAsync();
    }
}