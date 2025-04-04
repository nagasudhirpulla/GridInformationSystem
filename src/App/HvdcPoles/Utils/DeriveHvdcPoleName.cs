namespace App.HvdcPoles.Utils;

public static class DeriveHvdcPoleName
{
    public static string Execute(string substationName, string elementNumber)
    {
        string busName = $"HVDC {substationName} Pole-{elementNumber}";
        return busName;
    }
}