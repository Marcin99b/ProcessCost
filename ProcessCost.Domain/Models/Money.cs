using ProcessCost.Domain.Exceptions;

namespace ProcessCost.Domain.Models;

public record Money
{
    public int CalculationAmount { get; init; }
    public Currency Currency { get; init; }

    public Money()
    {
    }

    public Money(decimal amount, Currency currency)
    {
        this.Currency = currency;
        this.CalculationAmount = decimal.ToInt32(amount * 100);
    }

    public Money(int calculationAmount, Currency currency)
    {
        this.Currency = currency;
        this.CalculationAmount = calculationAmount;
    }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new CannotAddMoneyCausedByDifferentCurrencyException();
        }

        var sum = a.CalculationAmount + b.CalculationAmount;
        return new(sum, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new CannotSubtractMoneyCausedByDifferentCurrencyException();
        }

        var sum = a.CalculationAmount - b.CalculationAmount;
        return new(sum, a.Currency);
    }

    /// <summary>
    /// Examples: 10,00 PLN | 1,00 PLN | -0,50 PLN | 0,50 PLN | -10,50 PLN
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var amountAsText = this.CalculationAmount.ToString();
        if (amountAsText.Length == 2 || (amountAsText.StartsWith('-') && amountAsText.Length == 3))
        {
            amountAsText = amountAsText.Insert(amountAsText.Length - 2, "0");
        }

        amountAsText = amountAsText.Insert(amountAsText.Length - 2, ",");
        var result = $"{amountAsText} {this.Currency}";
        return result;
    }
}