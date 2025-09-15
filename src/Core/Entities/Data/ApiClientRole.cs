using Core.Entities.Common;

namespace Core.Entities.Data;

public class ApiClientRole : AuditableEntity
{
    public ApiClient ApiClient { get; set; } = null!;
    public int ApiClientId { get; set; }

    public ApiRole ApiRole { get; set; } = null!;
    public int ApiRoleId { get; set; }

}
