﻿using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;

public interface IStagesGroupsRepository
{
    public Task Add(StageGroup group);
    public Task Update(StageGroup group);
    public Task Delete(Guid groupId);
}