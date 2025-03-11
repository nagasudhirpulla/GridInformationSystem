using Core.Enums;

namespace App.Transformers.Utils;

public static class DeriveTransformerName
{
    public static string Execute(string location, string hvLevel, string lvLevel, string elNumber, TransformerTypeEnum transType)
    {
        string transName = $"{hvLevel}/{lvLevel} {location} {transType} - {elNumber}";
        return transName;
    }
}
