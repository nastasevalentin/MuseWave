using MuseWave.Domain.Common;

namespace MuseWave.Domain.Entities;

public class Album
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; } // Foreign Key to Artist
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverImage { get; set; }
    
    private Album(string title, Guid artistId, string genre, DateTime releaseDate, string coverImage)
    {
        Id = new Guid();
        Title = title;
        ArtistId = artistId;
        Genre = genre;
        ReleaseDate = releaseDate;
        CoverImage = coverImage;
    }
    
    public static Result<Album> Create(string title, Guid artistId, string genre, DateTime releaseDate, string coverImage)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<Album>.Failure("title should not be empty");
        }
        
        if (artistId == default)
        {
            return Result<Album>.Failure("artist id should not be default");
        }
        
        if (string.IsNullOrWhiteSpace(genre))
        {
            return Result<Album>.Failure("genre should not be empty");
        }
        
        if (releaseDate == default)
        {
            return Result<Album>.Failure("release date should not be default");
        }
        
        if (string.IsNullOrWhiteSpace(coverImage))
        {
            return Result<Album>.Failure("cover image should not be empty");
        }
        
        return Result<Album>.Success(new Album(title, artistId, genre, releaseDate, coverImage));
    }
    
    public void AttachArtist(Guid artistId)
    {
        ArtistId = artistId;
    }
}