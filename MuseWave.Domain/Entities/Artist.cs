namespace MuseWave.Domain.Entities;

public class Artist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Foreign Key to User
    public string StageName { get; set; }
    public string Biography { get; set; }
    public string ProfileImage { get; set; }
    
    private Artist(Guid userId, string stageName, string biography, string profileImage)
    {
        Id = new Guid();
        UserId = userId;
        StageName = stageName;
        Biography = biography;
        ProfileImage = profileImage;
    }
}