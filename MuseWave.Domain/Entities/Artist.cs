using MuseWave.Domain.Common;

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
    
    public static Result <Artist> Create(Guid userId, string stageName, string biography, string profileImage)
    {
        if (userId == default)
        {
            return Result<Artist>.Failure("user id should not be default");
        }
        
        if (string.IsNullOrWhiteSpace(stageName))
        {
            return Result<Artist>.Failure("stage name should not be empty");
        }
        
        if (string.IsNullOrWhiteSpace(biography))
        {
            return Result<Artist>.Failure("biography should not be empty");
        }
        
        if (string.IsNullOrWhiteSpace(profileImage))
        {
            return Result<Artist>.Failure("profile image should not be empty");
        }
        
        return Result<Artist>.Success(new Artist(userId, stageName, biography, profileImage));
    }
    public void AttachUser(Guid userId)
    {
        UserId = userId;
    }
    
}