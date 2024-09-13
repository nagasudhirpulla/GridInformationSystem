using Core.Entities;
using System.ComponentModel.DataAnnotations;

public class Constituent : AuditableEntity
{
    public required string Name { get; set; }
}
