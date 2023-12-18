namespace ProcessCost.Database.Entities;

public record StageGroupEntity : Entity
{
    public string Name { get; init; }
    public int MoneyAmount { get; init; }
    public string MoneyCurrency { get; init; }
}