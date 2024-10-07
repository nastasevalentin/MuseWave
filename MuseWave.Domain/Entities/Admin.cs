using MuseWave.Domain.Common;

namespace MuseWave.Domain.Entities;

public class Admin
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Foreign Key to User 
    private Admin(Guid userId)
    {
        Id = new Guid();
        UserId = userId;
    }
    
    public static Result<Admin> Create(Guid userId)
    {
        if (userId == default)
        {
            return Result<Admin>.Failure("user id should not be default");
        }
        
        return Result<Admin>.Success(new Admin(userId));
    }
    
    public void AttachUser(Guid userId)
    {
        UserId = userId;
    }
}