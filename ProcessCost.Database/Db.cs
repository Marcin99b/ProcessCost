using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProcessCost.Database
{
    public class Db : DbContext
    {
        public DbSet<StageEntity> Stages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ProcessCostDb");
        }
    }

    public record Entity
    {
        [Key]
        public Guid Id { get; set; }
    }

    public record StageEntity : Entity
    {
        public string Name { get; set; }
        public int Day { get; set; }
        public int MoneyAmount { get; set; }
        public string MoneyCurrency { get; set; }
    }
}
