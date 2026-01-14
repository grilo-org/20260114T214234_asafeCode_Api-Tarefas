namespace TarefasCrud.Domain.Extensions;

public static class DateExtensions
{
    public static int GetMonthWeek(this DateOnly startDate)
    {
        var firstDayOfMonth = new DateOnly(startDate.Year, startDate.Month, 1);

        var offset = ((int)DayOfWeek.Monday - (int)firstDayOfMonth.DayOfWeek + 7) % 7;

        var firstMonday = firstDayOfMonth.AddDays(offset);
        
        var week = (startDate.DayNumber - firstMonday.DayNumber) / 7 + 1;

        return week;
    }
    public static DateOnly ToDateOnly(this DateTime date) => DateOnly.FromDateTime(date);

    public static DateTime BrasiliaTz(this DateTime date)
    {
        var timeZoneId = OperatingSystem.IsWindows()
            ? "E. South America Standard Time"  
            : "America/Sao_Paulo";        
        
        var brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTimeFromUtc(date, brasiliaTimeZone);
    }
}