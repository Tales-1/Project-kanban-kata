namespace ProjectKanban.Tasks.Dtos;

public record TaskRecord
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public int EstimatedDevDays { get; set; }
}
