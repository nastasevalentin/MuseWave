namespace MuseWave.Domain.Entities;

public class Admin
{
    public Guid AdminId { get; set; }
    public Guid UserId { get; set; } // Foreign Key to User 
}