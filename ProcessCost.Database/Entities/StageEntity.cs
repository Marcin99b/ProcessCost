namespace ProcessCost.Database.Entities;

public record StageEntity : Entity
{
    public string Name { get; set; }
    public int Day { get; set; }
    public int MoneyAmount { get; set; }
    public string MoneyCurrency { get; set; }
}