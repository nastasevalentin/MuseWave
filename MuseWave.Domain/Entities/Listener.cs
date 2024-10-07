using MuseWave.Domain.Common;

namespace MuseWave.Domain.Entities;

public class Listener
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Foreign Key to User
    public DateTime DateOfBirth { get; set; }
    
    private Listener(Guid userId, DateTime dateOfBirth)
    {
        Id = new Guid();
        UserId = userId;
        DateOfBirth = dateOfBirth;
    }
    
    public static Result<Listener> Create(Guid userId, DateTime dateOfBirth)
    {
        if (userId == default)
        {
            return Result<Listener>.Failure("user id should not be default");
        }
        
        if (dateOfBirth == default)
        {
            return Result<Listener>.Failure("date of birth should not be default");
        }
        
        return Result<Listener>.Success(new Listener(userId, dateOfBirth));
    }
    
    public void AttachUser(Guid userId)
    {
        UserId = userId;
    }
}