using FluentAssertions;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Tests;

[TestFixture]
public class StageTests
{
    [TestCase(10, Currency.PLN, "10,00 PLN")]
    [TestCase(1, Currency.PLN, "1,00 PLN")]
    [TestCase(0.50, Currency.PLN, "0,50 PLN")]
    [TestCase(-0.50, Currency.PLN, "-0,50 PLN")]
    [TestCase(10.50, Currency.PLN, "10,50 PLN")]
    [TestCase(10.50, Currency.EUR, "10,50 EUR")]
    [TestCase(10.50, Currency.USD, "10,50 USD")]
    [TestCase(-10, Currency.PLN, "-10,00 PLN")]
    public void MoneyShouldConvertItselfToString(decimal amount, Currency currency, string expected)
    {
        //Arrange
        var money = new Money(amount, currency);

        //Act
        var text = money.ToString();
        //Assert
        text.Should().Be(expected);
    }

    [Test]
    public void AddingStageShouldCopyDayFromNextAndSumAlwaysInChronologicalOrder()
    {
        //Arrange
        var previous = new Stage("A", 5, new(5.5M, Currency.PLN));
        var next = new Stage("B", 10, new(-2M, Currency.PLN));

        //Act
        var resultA = previous.Add(next, "C");
        var resultB = next.Add(previous, "C");

        //Assert
        resultA.Should().BeEquivalentTo(resultB, x => x.Excluding(o => o.Id));
        resultA.Should().BeEquivalentTo(new Stage("C", 10, new(3.5M, Currency.PLN)), x => x.Excluding(o => o.Id));
    }

    [Test]
    public void AddingStageToGroupShouldRecalculateMoney()
    {
        //Arrange
        var stage = new Stage("A", 5, new(10M, Currency.PLN));
        var group = new StageGroup("B");

        //Act
        group.AddStage(stage);

        //Assert
        group.Money.CalculationAmount.Should().Be(10_00);
        group.StagesIds.Should().HaveCount(1);
    }

    [Test]
    public void RemovingStageFromGroupShouldRecalculateMoney()
    {
        //Arrange
        var stage = new Stage("A", 5, new(10M, Currency.PLN));
        var group = new StageGroup("B");
        group.AddStage(stage);

        //Act
        group.RemoveStage(stage);

        //Assert
        group.Money.CalculationAmount.Should().Be(0);
        group.StagesIds.Should().HaveCount(0);
    }

    [Test]
    public void GroupShouldProtectFromAddingSameStageTwice()
    {
        //Arrange
        var stage = new Stage("A", 5, new(10M, Currency.PLN));
        var group = new StageGroup("B");

        //Act
        group.AddStage(stage);
        var act = () => group.AddStage(stage);
        ;

        //Assert
        act.Should().Throw<Exception>().WithMessage("Group already contains stage");
    }

    [Test]
    public void GroupShouldProtectFromRemovingNotExistedStage()
    {
        //Arrange
        var stage = new Stage("A", 5, new(10M, Currency.PLN));
        var group = new StageGroup("B");

        //Act
        var act = () => group.RemoveStage(stage);

        //Assert
        act.Should().Throw<Exception>().WithMessage("Group doesn't contains stage");
    }
}