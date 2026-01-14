namespace CommonTestUtilities.Extensions;

public static class DatesExtensions
{
    public static DateOnly NextWeekday(this DateOnly currentDate, DayOfWeek targetDay)
    {
        if (currentDate.DayOfWeek == targetDay)
            return currentDate;

        while (currentDate.DayOfWeek != targetDay)
        {
            currentDate = currentDate.AddDays(1);
        }
    
        return currentDate;
    }    
    public static DateOnly PastWeekday(this DateOnly currentDate, DayOfWeek targetDay)
    {    
        // Começa do dia anterior
        var result = currentDate.AddDays(-1);
    
        // Retrocede até encontrar o targetDay
        while (result.DayOfWeek != targetDay)
        {
            result = result.AddDays(-1);
        }
    
        return result;
    }
    
    
}