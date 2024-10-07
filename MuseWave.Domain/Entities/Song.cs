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
}