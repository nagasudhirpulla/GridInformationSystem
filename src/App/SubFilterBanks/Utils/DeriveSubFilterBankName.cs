namespace App.SubFilterBanks.Utils;

public static class DeriveSubFilterBankName
{
    public static string Execute(string substationName, string filterBankNumber, string subFilterBankTag)
    {
        string subFilterBankName = $"{substationName} HVDC FilterBank-{filterBankNumber} Sub Filter {subFilterBankTag}";
        return subFilterBankName;
    }
}