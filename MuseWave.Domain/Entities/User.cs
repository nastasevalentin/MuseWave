namespace MuseWave.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public DateTime DateJoined { get; set; }
}

public enum Role
{
    Admin,
    Artist,
    Listener
}