using Core.Entities;
using Core.Entities.Common;
using Core.Entities.Elements;

namespace App.GridEntities.Dtos;

public class GridEntityDto : AuditableEntity
{
    public required string Name { get; set; }

    public static GridEntityDto GetFromGridEntity(GridEntity e)
    {
        string name = "";
        switch (e)
        {
            case Element element:
                name = element.ElementNameCache;
                break;
            case Fuel f:
                name = f.FuelName;
                break;
            case Location l:
                name = l.Name;
                break;
            case Owner o:
                name = o.Name;
                break;
            case Region r:
                name = r.Name;
                break;
            case State s:
                name = s.Name;
                break;
            case Substation sub:
                name = sub.NameCache;
                break;
        }

        return new GridEntityDto()
        {
            CreatedBy = e.CreatedBy,
            Created = e.Created,
            LastModified = e.LastModified,
            LastModifiedBy = e.LastModifiedBy,
            Name = name
        };
    }
}
