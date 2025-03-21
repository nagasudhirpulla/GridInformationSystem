namespace App.BusReactors.Utils;

public static class DeriveBusReactorName
{
    public static string Execute(string substationName, string elementNumber)
    {
        string brName = $"{substationName} BR-{elementNumber}";
        return brName;
    }
}
