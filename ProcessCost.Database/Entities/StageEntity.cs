namespace ProcessCost.Database.Entities;

public record StageEntity : Entity
{
    public string Name { get; init; }
    public int Day { get; init; }
    public int MoneyAmount { get; init; }
    public string MoneyCurrency { get; init; }
}

public record StageGroupEntity : Entity
{
    public string Name { get; init; }
    public int MoneyAmount { get; init; }
    public string MoneyCurrency { get; init; }
}

public record StageGroupReferenceEntity : Entity
{
    public Guid StageId { get; init; }
    public Guid StageGroupId { get; init; }
}