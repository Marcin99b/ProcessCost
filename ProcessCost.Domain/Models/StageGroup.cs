namespace ProcessCost.Domain.Models;

public class StageGroup(string name, Money money, IEnumerable<Guid> stagesIds)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; } = name;
    public Money Money { get; } = money;
    public IEnumerable<Guid> StagesIds { get; } = stagesIds;

    public void AddStage(Stage stage)
    {

    }

    public void RemoveStage(Stage stage)
    {

    }
} 