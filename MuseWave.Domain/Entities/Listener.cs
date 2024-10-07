namespace MuseWave.Domain.Entities;

public class Listener
{
    public Guid ListenerId { get; set; }
    public Guid UserId { get; set; } // Foreign Key to User
    public DateTime DateOfBirth { get; set; }
}