using Microsoft.EntityFrameworkCore;
using ProcessCost.Database.Entities;

namespace ProcessCost.Database;

public class DatabaseContext : DbContext
{
    public DbSet<StageEntity> Stages { get; set; }
    public DbSet<StageGroupEntity> StagesGropus { get; set; }
    public DbSet<StageGroupReferenceEntity> StagesGropusReferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseInMemoryDatabase("ProcessCostDb");
    }
}