namespace App.Substations.Utils;
public static class SubstationUtils
{
    public static string DeriveSubstationName(string voltageLevel, string location)
    {
        return $"{voltageLevel} {location}";
    }
}