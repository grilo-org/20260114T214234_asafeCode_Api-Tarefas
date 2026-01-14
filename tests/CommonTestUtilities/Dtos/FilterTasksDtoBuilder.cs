using TarefasCrud.Domain.Dtos;

namespace CommonTestUtilities.Dtos;

public static class FilterTasksDtoBuilder
{
    public static string BuildQuery(FilterTasksDto filter)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query.Add($"Title={Uri.EscapeDataString(filter.Title)}");

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query.Add($"Category={Uri.EscapeDataString(filter.Category)}");

        if (filter.IsCompleted.HasValue)
            query.Add($"IsCompleted={filter.IsCompleted.Value.ToString().ToLower()}");

        if (filter.WeeklyGoalMin.HasValue)
            query.Add($"WeeklyGoalMin={filter.WeeklyGoalMin}");

        if (filter.WeeklyGoalMax.HasValue)
            query.Add($"WeeklyGoalMax={filter.WeeklyGoalMax}");

        if (filter.ProgressMin.HasValue)
            query.Add($"ProgressMin={filter.ProgressMin}");

        if (filter.ProgressMax.HasValue)
            query.Add($"ProgressMax={filter.ProgressMax}");

        if (filter.WeekOfMonth.HasValue)
            query.Add($"WeekOfMonth={filter.WeekOfMonth}");

        if (filter.Month.HasValue)
            query.Add($"Month={filter.Month}");

        if (filter.Year.HasValue)
            query.Add($"Year={filter.Year}");

        return query.Count == 0
            ? string.Empty
            : "?" + string.Join("&", query);
    }
}