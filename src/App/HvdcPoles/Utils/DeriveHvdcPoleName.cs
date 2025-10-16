namespace App.HvdcPoles.Utils;

public static class DeriveHvdcPoleName
{
    public static string Execute(string substationName, string elementNumber)
    {
        string hvdcPoleName = $"HVDC {substationName} Pole-{elementNumber}";
        return hvdcPoleName;
    }
}