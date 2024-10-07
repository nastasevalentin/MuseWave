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
}