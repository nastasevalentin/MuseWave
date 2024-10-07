using MuseWave.Domain.Common;

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
    
    public static Result <User> Create(string username, string email, string password, Role role, DateTime dateJoined)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return Result<User>.Failure("username is required");
        }
        
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<User>.Failure("email is required");
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result<User>.Failure("password is required");
        }
        
        if (role == default)
        {
            return Result<User>.Failure("role should not be default");
        }
        
        if (dateJoined == default)
        {
            return Result<User>.Failure("date joined should not be default");
        }
        
        return Result<User>.Success(new User(username, email, password, role, dateJoined));
    }
}

public enum Role
{
    Admin,
    Artist,
    Listener
}