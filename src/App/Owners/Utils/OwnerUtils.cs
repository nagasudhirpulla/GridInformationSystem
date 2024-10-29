using Core.Entities;

namespace App.Owners.Utils;

public static class OwnerUtils
{
    public static string DeriveOwnersCache(IList<Owner> owners)
    {
        string ownersCache = String.Join("", owners.Select(o => o.Name + ";"));
        return ownersCache;
    }
}