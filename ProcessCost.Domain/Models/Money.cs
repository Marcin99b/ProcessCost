namespace ProcessCost.Domain.Models;

public record Money
{
    public int CalculationAmount { get; }
    public Currency Currency { get; }

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
            throw new ArgumentException("Money must have same currency");
        }

        var sum = a.CalculationAmount + b.CalculationAmount;
        return new(sum, a.Currency);
    }

    public static Money operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new ArgumentException("Money must have same currency");
        }

        var sum = a.CalculationAmount - b.CalculationAmount;
        return new(sum, a.Currency);
    }

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