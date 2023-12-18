using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;

public interface IStagesRepository
{
    public Stage GetStageById(Guid stageId);
    public IEnumerable<Stage> GetAllStagesOfUser(Guid userId);

    public Task Add(Stage stage);
    public Task Update(Stage stage);

    public Task Delete(Guid stageId);
}