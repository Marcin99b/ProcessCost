using FluentAssertions;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain.Tests
{
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
            var previous = new Stage(5, new Money(5.5M, Currency.PLN));
            var next = new Stage(10, new Money(-2M, Currency.PLN));

            //Act
            var resultA = previous.Add(next);
            var resultB = next.Add(previous);

            //Assert
            resultA.Should().BeEquivalentTo(resultB);
            resultA.Should().BeEquivalentTo(new Stage(10, new Money(3.5M, Currency.PLN)));
        }
    }
}
