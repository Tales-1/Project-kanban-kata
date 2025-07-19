namespace ProjectKanban.Users.Dtos;

public record UserRecord
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
