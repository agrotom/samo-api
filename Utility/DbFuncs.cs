namespace Samo.Utility;

public static class DbFuncs
{
    public static DateTime ToLocalTime(DateTime timestamp, string timeZone)
        => throw new NotSupportedException("Only for use with EF Core query translation.");
}