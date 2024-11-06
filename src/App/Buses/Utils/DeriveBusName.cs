using Core.Enums;

namespace App.Buses.Utils;

public static class DeriveBusName
{
    public static string Execute(string substationName, BusTypeEnum busType, string elementNumber)
    {
        string busTypeName = $"{busType.Name} Bus";
        if (busType == BusTypeEnum.Main)
        {
            busTypeName = "Bus";
        }
        string busName = $"{substationName} {busTypeName}-{elementNumber}";
        return busName;

    }
}