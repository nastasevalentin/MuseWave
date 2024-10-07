namespace MuseWave.Domain.Entities;

public class Artist
{
    public Guid ArtistId { get; set; }
    public Guid UserId { get; set; } // Foreign Key to User
    public string StageName { get; set; }
    public string Biography { get; set; }
    public string ProfileImage { get; set; }
}