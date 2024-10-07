namespace MuseWave.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public DateTime DateJoined { get; set; }
    
    private User(string username, string email, string password, Role role, DateTime dateJoined)
    {
        UserId = new Guid();
        Username = username;
        Email = email;
        Password = password;
        Role = role;
        DateJoined = dateJoined;
    } 
}

public enum Role
{
    Admin,
    Artist,
    Listener
}