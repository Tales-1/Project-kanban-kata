namespace ProjectKanban.Tasks.Dtos;

public record TaskAssignedRecord
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
}
