using System.ComponentModel.DataAnnotations;

namespace ProcessCost.Database.Entities;

public record Entity
{
    [Key] public Guid Id { get; set; }
}