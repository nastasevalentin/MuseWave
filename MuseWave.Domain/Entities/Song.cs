using MuseWave.Domain.Common;

namespace MuseWave.Domain.Entities;

public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; } // Foreign Key to Artist
    public Guid? AlbumId { get; set; } // Foreign Key to Album, Nullable
    public string Genre { get; set; }
    public string AudioFile { get; set; }
    public DateTime ReleaseDate { get; set; }
    
    private Song(string title, Guid artistId, Guid? albumId, string genre, string audioFile, DateTime releaseDate)
    {
        Id = new Guid();
        Title = title;
        ArtistId = artistId;
        AlbumId = albumId;
        Genre = genre;
        AudioFile = audioFile;
        ReleaseDate = releaseDate;
    }
    
    public static Result<Song> Create(string title, Guid artistId, Guid? albumId, string genre, string audioFile, DateTime releaseDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<Song>.Failure("title should not be empty");
        }
        
        if (artistId == default)
        {
            return Result<Song>.Failure("artist id should not be default");
        }
        
        if (string.IsNullOrWhiteSpace(genre))
        {
            return Result<Song>.Failure("genre should not be empty");
        }
        
        if (string.IsNullOrWhiteSpace(audioFile))
        {
            return Result<Song>.Failure("audio file should not be empty");
        }
        
        if (releaseDate == default)
        {
            return Result<Song>.Failure("release date should not be default");
        }
        
        return Result<Song>.Success(new Song(title, artistId, albumId, genre, audioFile, releaseDate));
    }
    
    public void AttachArtist(Guid artistId)
    {
        ArtistId = artistId;
    }
    
    public void AttachAlbum(Guid albumId)
    {
        AlbumId = albumId;
    }
}