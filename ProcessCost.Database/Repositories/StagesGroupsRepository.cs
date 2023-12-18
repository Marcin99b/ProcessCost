using ProcessCost.Database.Entities;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;

public class StagesGroupsRepository(DatabaseContext context) : IStagesGroupsRepository
{
    public StageGroup GetById(Guid groupId)
    {
        var groupEntity = context.StagesGropus.First(x => x.Id == groupId);
        var refs = context.StagesGropusReferences.Where(x => x.StageGroupId == groupId);

        var group = new StageGroup(groupEntity.Name)
        {
            Id = groupEntity.Id,
            Money = new(groupEntity.MoneyAmount, Enum.Parse<Currency>(groupEntity.MoneyCurrency)),
            StagesIds = refs.Select(x => x.StageId)
        };

        return group;
    }

    public async Task Create(StageGroup group)
    {
        if (group.StagesIds.Any())
        {
            await this.UpdateReferences(group);
        }

        var groupEntity = new StageGroupEntity
        {
            Id = group.Id,
            Name = group.Name,
            MoneyAmount = 0,
            MoneyCurrency = group.Money.Currency.ToString(),
        };
        await context.StagesGropus.AddAsync(groupEntity);
        await context.SaveChangesAsync();
    }

    public async Task Update(StageGroup group)
    {
        await this.UpdateReferences(group);

        var groupEntity = new StageGroupEntity
        {
            Id = group.Id,
            Name = group.Name,
            MoneyAmount = group.Money.CalculationAmount,
            MoneyCurrency = group.Money.Currency.ToString(),
        };

        context.StagesGropus.Update(groupEntity);
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

    private async Task UpdateReferences(StageGroup group)
    {
        var registeredStagesInCurrentGroup =
            context.StagesGropusReferences.Where(x => x.StageGroupId == group.Id);

        foreach (var groupInDatabase in registeredStagesInCurrentGroup)
        {
            if (!group.StagesIds.Contains(groupInDatabase.StageId))
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
    }
}