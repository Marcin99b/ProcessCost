namespace ProcessCost.Database.Entities;

public record StageGroupReferenceEntity : Entity
{
    public Guid StageId { get; init; }
    public Guid StageGroupId { get; init; }
}