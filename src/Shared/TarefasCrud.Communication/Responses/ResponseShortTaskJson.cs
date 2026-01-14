namespace TarefasCrud.Communication.Responses;

public class ResponseShortTaskJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int WeeklyGoal { get; set; }
    public int Progress { get; set; }
    public string Category { get; set; } = string.Empty;
    public int WeekOfMonth { get; set; } 
}