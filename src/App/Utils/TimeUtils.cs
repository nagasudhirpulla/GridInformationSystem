namespace App.Utils;

public class TimeUtils
{
    public static int GetUtcMs(DateTime dt)
    {
        return (int)dt.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    public static DateTime UtcMsToDateTime(int unixTimeStampMs)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTimeStampMs).ToLocalTime();
        return dateTime;
    }
}
