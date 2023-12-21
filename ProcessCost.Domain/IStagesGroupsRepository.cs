using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;

public interface IStagesGroupsRepository
{
    public Task<StageGroup?> GetById(Guid groupId);
    public Task Create(StageGroup group);
    public Task Update(StageGroup group);
    public Task Delete(Guid groupId);
}