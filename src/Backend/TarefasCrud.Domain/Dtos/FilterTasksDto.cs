namespace TarefasCrud.Domain.Dtos;

public class FilterTasksDto
{
    public string? Title { get; set; }
    public string? Category { get; set; }
    
    public bool? IsCompleted { get; set; }
    public int? WeeklyGoalMin { get; set; }
    public int? WeeklyGoalMax { get; set; }
    public int? ProgressMin { get; set; }
    public int? ProgressMax { get; set; }

    public int? WeekOfMonth { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
}