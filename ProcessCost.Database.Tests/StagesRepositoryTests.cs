using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProcessCost.Database.Entities;
using ProcessCost.Database.Repositories;
using ProcessCost.Domain.Models;
using System.Text.RegularExpressions;

namespace ProcessCost.Database.Tests;

[TestFixture]
public class StagesRepositoryTests
{
    [Test]
    public async Task ShouldGetStageById()
    {
        //Arrange
        var context = this.CreateContext();
        var id = Guid.NewGuid();
        await context.Stages.AddRangeAsync(
        [
            new()
            {
                Id = Guid.NewGuid(),
                Day = 1,
                MoneyAmount = 50_00,
                MoneyCurrency = "PLN",
                Name = "Test",
            },
            new()
            {
                Id = id,
                Day = 5,
                MoneyAmount = 20_00,
                MoneyCurrency = "USD",
                Name = "A",
            },
        ]);
        await context.SaveChangesAsync();
        var repository = new StagesRepository(context);

        //Act
        var result = await repository.GetStageById(id)!;

        //Assert
        context.Stages.Should().NotBeEmpty();
        result.Should()
            .BeEquivalentTo(new Stage("A", 5, new(20M, Currency.USD))
            {
                Id = id,
            });
    }

    [Test]
    public async Task ShouldGetAllStagesOfUser()
    {
        //Arrange
        var context = this.CreateContext();
        await context.Stages.AddRangeAsync(
        [
            new()
            {
                Id = Guid.NewGuid(),
                Day = 1,
                MoneyAmount = -50_00,
                MoneyCurrency = "PLN",
                Name = "Test",
            },
            new()
            {
                Id = Guid.NewGuid(),
                Day = 5,
                MoneyAmount = 20_00,
                MoneyCurrency = "USD",
                Name = "A",
            },
        ]);
        await context.SaveChangesAsync();
        var repository = new StagesRepository(context);

        //Act
        var stages = repository.GetAllStagesOfUser(Guid.NewGuid()).ToArray();

        //Assert
        context.Stages.Should().HaveCount(2);
        stages.Should().HaveCount(2);
        stages[0].Name.Should().Be("Test");
        stages[0].Day.Should().Be(1);
        stages[0].Money.CalculationAmount.Should().Be(-50_00);
        stages[1].Name.Should().Be("A");
        stages[1].Day.Should().Be(5);
        stages[1].Money.CalculationAmount.Should().Be(20_00);
    }

    [Test]
    public async Task ShouldAdd()
    {
        //Arrange
        var context = this.CreateContext();
        var repository = new StagesRepository(context);
        var stage = new Stage("Test", 5, new(50_00, Currency.USD));

        //Act
        await repository.Add(stage);

        //Assert
        context.Stages.Should().NotBeEmpty();
        context.Stages.First()!
            .Should()
            .BeEquivalentTo(new StageEntity
            {
                Id = stage.Id,
                Day = 5,
                Name = "Test",
                MoneyAmount = 50_00,
                MoneyCurrency = "USD",
            });
    }

    [Test]
    public async Task ShouldUpdate()
    {
        //Arrange
        var context = this.CreateContext();
        var repository = new StagesRepository(context);
        var stage = new Stage("Test", 5, new(50_00, Currency.USD));
        await repository.Add(stage);

        //Act
        stage.UpdateMoney(new(-10_00, Currency.EUR));
        await repository.Update(stage);
        var result = await repository.GetStageById(stage.Id);

        //Assert
        context.Stages.Should().NotBeEmpty();
        result!.Money.Should().BeEquivalentTo(new Money(-10_00, Currency.EUR));
    }

    [Test]
    public async Task ShouldDelete()
    {
        //Arrange
        var context = this.CreateContext();
        var repository = new StagesRepository(context);
        var stage = new Stage("Test", 5, new(50_00, Currency.USD));
        await repository.Add(stage);

        //Act
        await repository.Delete(stage.Id);

        //Assert
        context.Stages.Should().BeEmpty();
    }

    private DatabaseContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new(options);
    }
}