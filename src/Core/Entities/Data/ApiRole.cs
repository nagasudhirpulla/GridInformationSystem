using Core.Entities.Common;

namespace Core.Entities.Data;

public class ApiRole : AuditableEntity
{
    public required string Name { get; set; }
    public List<ApiClientRole> ApiClientRoles { get; } = [];
}
