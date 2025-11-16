namespace Samo.Utility;

public static class Dates
{
    public static DateTime TodayDate()
    {
        return DateTime.Today.ToUniversalTime().Date;
    }

    public static bool IsToday(DateTime date1)
    {
        return date1.ToUniversalTime().Date == TodayDate();
    }
}