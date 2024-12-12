using MuseWave.Domain.Common;

namespace MuseWave.Domain.Entities;

public class Album
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; }
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string CoverImage { get; set; }
    
    public Album(string title, Guid artistId, string genre, DateTime releaseDate, string coverImage)
    {
        Id = new Guid();
        Title = title;
        ArtistId = artistId;
        Genre = genre;
        ReleaseDate = releaseDate;
        CoverImage = coverImage;
    }
    
    public Album()
    {
    }
    
    public void Update(string title, Guid artistId, string genre, DateTime releaseDate, string coverImage)
    {
        Title = title;
        ArtistId = artistId;
        Genre = genre;
        ReleaseDate = releaseDate;
        CoverImage = coverImage;
    }
    
}