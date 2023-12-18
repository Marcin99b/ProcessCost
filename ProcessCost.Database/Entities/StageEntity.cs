namespace ProcessCost.Database.Entities;

public record StageEntity : Entity
{
    public string Name { get; init; }
    public int Day { get; init; }
    public int MoneyAmount { get; init; }
    public string MoneyCurrency { get; init; }
}