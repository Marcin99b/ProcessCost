using Microsoft.EntityFrameworkCore;
using ProcessCost.Database.Entities;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ProcessCost.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<StageEntity> Stages { get; set; }
    public DbSet<StageGroupEntity> StagesGroups { get; set; }
    public DbSet<StageGroupReferenceEntity> StagesGroupsReferences { get; set; }
}