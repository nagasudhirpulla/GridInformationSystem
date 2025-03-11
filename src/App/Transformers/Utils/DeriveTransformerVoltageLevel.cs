namespace App.Transformers.Utils;

public static class DeriveTransformerVoltageLevel
{
    public static string Execute(string hvLevel, string lvLevel)
    {
        return $"{hvLevel}/{lvLevel}";
    }
}
