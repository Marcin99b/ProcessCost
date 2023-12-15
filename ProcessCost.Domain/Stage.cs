namespace ProcessCost.Domain
{
    public class Stage(int day, Money money)
    {
        public int Day { get; } = day;
        public Money Money { get; } = money;

        public Stage Add(Stage anotherStage)
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
            return new Stage(next.Day, previous.Money + next.Money);
        }
    }
}
