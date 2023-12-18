namespace ProcessCost.Domain.Models;

public class StageGroup(string name, Money money, ISet<Guid> stagesIds)
{
    private readonly ISet<Guid> _stagesIds = stagesIds;
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; } = name;
    public Money Money { get; private set; } = money;
    public IEnumerable<Guid> StagesIds => this._stagesIds;

    public void AddStage(Stage stage)
    {
        this.Money += stage.Money;
        this._stagesIds.Add(stage.Id);
    }

    public void RemoveStage(Stage stage)
    {
        this.Money -= stage.Money;
        this._stagesIds.Remove(stage.Id);
    }
}