using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;
public interface IStagesRepository
{
    public Task<Stage> GetStageById(Guid stageId);
    public Task<IEnumerable<Stage>> GetAllStagesOfUser(Guid userId);
}
