namespace App.GeneratingUnits.Utils;

public static class DeriveGenUnitName
{
    public static string Execute(string substationName, string elementNumber)
    {
        string elName = $"{substationName} Unit {elementNumber}";
        return elName;
    }
}
