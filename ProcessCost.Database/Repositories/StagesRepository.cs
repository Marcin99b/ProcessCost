using ProcessCost.Domain;
using ProcessCost.Domain.Models;

namespace ProcessCost.Database.Repositories;
public class StagesRepository : IStagesRepository
{
    private readonly Stage[] _db =
    [
        new("A", 01, new(10M, Currency.PLN)),
        new("A", 05, new(10M, Currency.PLN)),
        new("A", 10, new(100M, Currency.PLN)),
        new("A", 12, new(-80M, Currency.PLN)),
        new("A", 12, new(50M, Currency.PLN)),
        new("A", 15, new(200M, Currency.PLN)),
        new("A", 16, new(-30M, Currency.PLN)),
        new("A", 19, new(-5M, Currency.PLN)),
        new("A", 21, new(10M, Currency.PLN)),
    ];

    public async Task<Stage> GetStageById(Guid stageId)
    {
        await Task.CompletedTask;
        return this._db[0];
    }

    public async Task<IEnumerable<Stage>> GetAllStagesOfUser(Guid userId)
    {
        await Task.CompletedTask;
        return this._db.ToArray();
    }
}
