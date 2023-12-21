using System.ComponentModel.DataAnnotations;

namespace ProcessCost.Database.Entities;

public abstract record Entity
{
    [Key] public Guid Id { get; set; }
}