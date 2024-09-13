using System.Xml.Linq;

namespace Core.Entities;

public class OwnerElementRelation : AuditableEntity
{
    public int OwnerId {  get; set; }

    public required Owner Owner { get; set; }
}