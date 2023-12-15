namespace ProcessCost.Domain.Models
{
    public class Stage(string name, int day, Money money)
    {
        public string Name { get; } = name;
        public int Day { get; } = day;
        public Money Money { get; } = money;


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
            return new Stage(newName, next.Day, previous.Money + next.Money);
        }
    }
}
