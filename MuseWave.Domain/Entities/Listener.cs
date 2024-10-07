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
}