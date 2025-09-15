using Core.Entities.Common;

namespace Core.Entities.Data;

public class ApiClient : AuditableEntity
{
    public required string Name { get; set; }
    public required string Key { get; set; }
    public List<ApiClientRole> ApiClientRoles { get; } = [];
}
