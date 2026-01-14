namespace TarefasCrud.Communication.Responses;

public class ResponseTaskJson
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int WeeklyGoal { get; set; }
    public int Progress { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public int WeekOfMonth { get; set; }
    public bool IsCompleted { get; set; } = false;
}