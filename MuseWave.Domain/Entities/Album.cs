namespace MuseWave.Domain.Entities;

public class Album
{
    public Guid AlbumId { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; } // Foreign Key to Artist
    public string genre { get; set; }
    public DateTime releaseDate { get; set; }
    public string CoverImage { get; set; }
}