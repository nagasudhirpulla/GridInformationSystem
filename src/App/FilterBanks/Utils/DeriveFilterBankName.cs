namespace App.FilterBanks.Utils;

public static class DeriveFilterBankName
{
    public static string Execute(string substationName, string elementNumber)
    {
        string elName = $"HVDC {substationName} FilterBank-{elementNumber}";
        return elName;
    }
}
