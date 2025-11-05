namespace App.LineReactors.Utils;

public static class DeriveLineReactorName
{
    public static string Execute(string lineName, string substationName, string elementNumber)
    {
        return $"{lineName} LR-{elementNumber} at {substationName}";
    }
}