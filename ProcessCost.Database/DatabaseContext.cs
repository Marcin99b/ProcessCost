using Microsoft.EntityFrameworkCore;
using ProcessCost.Database.Entities;

namespace ProcessCost.Database;

public class DatabaseContext : DbContext
{
    public DbSet<StageEntity> Stages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "ProcessCostDb");
    }
}