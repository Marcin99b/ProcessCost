namespace ProcessCost.Domain.Models;

public class Stage(string name, int day, Money money)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = name;
    public int Day { get; init; } = day;
    public Money Money { get; private set; } = money;

    public Stage Add(Stage anotherStage, string newName = "")
    {
        Stage previous;
        Stage next;
        if (this.Day <= anotherStage.Day)
        {
            previous = this;
            next = anotherStage;
        }
        else
        {
            previous = anotherStage;
            next = this;
        }

        return new(newName, next.Day, previous.Money + next.Money);
    }

    public void UpdateMoney(Money newMoney)
    {
        this.Money = newMoney;
    }
}