using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProcessCost.Database.Repositories;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Tests;

[TestFixture]
public class StagesGroupsRepositoryTests
{
    [Test]
    public async Task ShouldGetById()
    {
        //Arrange
        var context = this.CreateContext();
        var groupId = Guid.NewGuid();
        var stage1Id = Guid.NewGuid();
        var stage2Id = Guid.NewGuid();
        await context.StagesGroups.AddAsync(new()
        {
            Id = groupId,
            MoneyAmount = 50_00,
            MoneyCurrency = "USD",
            Name = "Group",
        });
        await context.StagesGroupsReferences.AddRangeAsync(
        [
            new()
            {
                Id = Guid.NewGuid(),
                StageGroupId = groupId,
                StageId = stage1Id,
            },
            new()
            {
                Id = Guid.NewGuid(),
                StageGroupId = groupId,
                StageId = stage2Id,
            },
        ]);
        await context.SaveChangesAsync();
        var repository = new StagesGroupsRepository(context);

        //Act
        var result = (await repository.GetById(groupId))!;

        //Assert
        result.Should().BeEquivalentTo(new StageGroup("Group")
        {
            Id = groupId,
            Money = new(50_00, Currency.USD),
            StagesIds = new []{stage1Id, stage2Id,},
        });
    }

    [Test]
    public async Task ShouldCreate()
    {
        //Arrange
        var context = this.CreateContext();
        var stageGroup = new StageGroup("Group")
        {
            Id = Guid.NewGuid(),
            Money = new(50_00, Currency.USD),
            StagesIds = new[] { Guid.NewGuid(), Guid.NewGuid(), },
        };
        var repository = new StagesGroupsRepository(context);

        //Act
        await repository.Create(stageGroup);
        var result = await repository.GetById(stageGroup.Id);

        //Assert
        result.Should().BeEquivalentTo(stageGroup);
    }

    [Test]
    public async Task ShouldUpdate()
    {
        //Arrange
        var context = this.CreateContext();
        var stageGroup = new StageGroup("Group")
        {
            Id = Guid.NewGuid(),
            Money = new(50_00, Currency.USD),
            StagesIds = new[] { Guid.NewGuid(), Guid.NewGuid(), },
        };
        var repository = new StagesGroupsRepository(context);
        await repository.Create(stageGroup);
        var newStage = new Stage("abc", 5, new Money(10_00, Currency.USD));

        //Act
        stageGroup.AddStage(newStage);
        await repository.Update(stageGroup);
        var result = (await repository.GetById(stageGroup.Id))!;

        //Assert
        result.Should().BeEquivalentTo(stageGroup);
        result.Money.CalculationAmount.Should().Be(60_00);
        result.StagesIds.Should().HaveCount(3);
    }

    [Test]
    public async Task ShouldDelete()
    {
        //Arrange
        var context = this.CreateContext();
        var stageGroup = new StageGroup("Group")
        {
            Id = Guid.NewGuid(),
            Money = new(50_00, Currency.USD),
            StagesIds = new[] { Guid.NewGuid(), Guid.NewGuid(), },
        };
        var repository = new StagesGroupsRepository(context);
        await repository.Create(stageGroup);

        //Act
        context.StagesGroups.Should().NotBeEmpty();
        context.StagesGroupsReferences.Should().NotBeEmpty();
        await repository.Delete(stageGroup.Id);

        //Assert
        context.StagesGroups.Should().BeEmpty();
        context.StagesGroupsReferences.Should().BeEmpty();
    }

    private DatabaseContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new(options);
    }
}