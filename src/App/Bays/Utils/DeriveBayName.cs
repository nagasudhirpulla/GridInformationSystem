using Core.Enums;

namespace App.Bays.Utils;

public static class DeriveBayName
{
    public static string Execute(string el1Name, string el2Name, BayTypeEnum bayType)
    {
        string bayName = $"{bayType.Name} - {el1Name} AND {el2Name}";
        return bayName;
    }
}
