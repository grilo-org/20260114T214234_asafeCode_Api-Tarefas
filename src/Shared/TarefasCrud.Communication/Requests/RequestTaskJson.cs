namespace TarefasCrud.Communication.Requests;

public class RequestTaskJson
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int WeeklyGoal { get; set; }
    
    public string Category { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
}