using Microsoft.EntityFrameworkCore;
using ProcessCost.Database.Entities;
using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;

//todo performance tests
public class StagesGroupsRepository(DatabaseContext context) : IStagesGroupsRepository
{
    public async Task<StageGroup?> GetById(Guid groupId)
    {
        var groupEntity = await context.StagesGroups
            .FirstOrDefaultAsync(x => x.Id == groupId);
        if (groupEntity == null)
        {
            return null;
        }

        var refs = context.StagesGroupsReferences
            .Where(x => x.StageGroupId == groupId);

        var group = new StageGroup(groupEntity.Name)
        {
            Id = groupEntity.Id,
            Money = new(groupEntity.MoneyAmount, Enum.Parse<Currency>(groupEntity.MoneyCurrency)),
            StagesIds = refs.Select(x => x.StageId),
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
            MoneyAmount = group.Money.CalculationAmount,
            MoneyCurrency = group.Money.Currency.ToString(),
        };
        await context.StagesGroups.AddAsync(groupEntity);
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

        var found = (await context.StagesGroups.FindAsync(groupEntity.Id))!;
        context.StagesGroups.Entry(found).CurrentValues.SetValues(groupEntity);
        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid groupId)
    {
        var refs = context.StagesGroupsReferences.Where(x => x.StageGroupId == groupId);
        context.StagesGroupsReferences.RemoveRange(refs);

        var group = context.StagesGroups.First(x => x.Id == groupId);
        context.StagesGroups.Remove(group);

        await context.SaveChangesAsync();
    }

    private async Task UpdateReferences(StageGroup group)
    {
        var registeredStagesInCurrentGroup =
            context.StagesGroupsReferences.Where(x => x.StageGroupId == group.Id);

        foreach (var groupInDatabase in registeredStagesInCurrentGroup)
        {
            if (!group.StagesIds.Contains(groupInDatabase.StageId))
            {
                context.StagesGroupsReferences.Remove(groupInDatabase);
            }
        }

        foreach (var stageId in group.StagesIds)
        {
            if (!registeredStagesInCurrentGroup.Any(x => x.StageId == stageId))
            {
                await context.StagesGroupsReferences.AddAsync(new()
                {
                    Id = Guid.NewGuid(),
                    StageGroupId = group.Id,
                    StageId = stageId,
                });
            }
        }
    }
}