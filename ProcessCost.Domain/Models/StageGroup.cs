using ProcessCost.Domain.Exceptions;

namespace ProcessCost.Domain.Models;

public class StageGroup(string name)
{
    private readonly ISet<Guid> _stagesIds = new HashSet<Guid>();
    private Money _money = new(0, Currency.PLN);

    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; } = name;

    public Money Money
    {
        get => this._money;
        init => this._money = value;
    }

    public IEnumerable<Guid> StagesIds
    {
        get => this._stagesIds;
        init => this._stagesIds = value.ToHashSet();
    }

    public void AddStage(Stage stage)
    {
        if (this._stagesIds.Contains(stage.Id))
        {
            throw new StageGroupAlreadyContainsStageException();
        }

        this._money += stage.Money;
        this._stagesIds.Add(stage.Id);
    }

    public void RemoveStage(Stage stage)
    {
        if (!this._stagesIds.Contains(stage.Id))
        {
            throw new StageGroupNotContainsStageException();
        }

        this._money -= stage.Money;
        this._stagesIds.Remove(stage.Id);
    }
}