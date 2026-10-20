namespace Samo.Utility;

public static class Dates
{
    public static DateTime TodayDate()
    {
        return DateTime.Today.ToUniversalTime().Date;
    }

    public static DateTime[] TodayLimit(string timeZone)
    {
        TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzInfo);
        var todayLocal = nowLocal.Date;

        var startLocal = todayLocal;
        var endLocal = todayLocal.AddDays(1);

        var startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, tzInfo);
        var endUtc   = TimeZoneInfo.ConvertTimeToUtc(endLocal, tzInfo);

        return [ startUtc, endUtc ];
    }

    public static DateTime[] TodayLimit(DateTime date, string timeZone)
    {
        var newDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Unspecified);
        return [ TimeZoneInfo.ConvertTimeToUtc(newDate, TimeZoneInfo.FindSystemTimeZoneById(timeZone)),
                    TimeZoneInfo.ConvertTimeToUtc(newDate.AddDays(1), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) ];
    }

    public static bool IsToday(DateTime date1)
    {
        return date1.ToUniversalTime().Date == TodayDate();
    }
}