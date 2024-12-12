using System.ComponentModel.DataAnnotations;
using MuseWave.Domain.Common;

namespace MuseWave.Domain.Entities;

public class Song
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; } 
    public Guid? AlbumId { get; set; }
    public string Genre { get; set; }
    public string AudioFile { get; set; }
    public DateTime ReleaseDate { get; set; }
    
    public Song(string title, Guid artistId, Guid? albumId, string genre, string audioFile, DateTime releaseDate)
    {
        Id = new Guid();
        Title = title;
        ArtistId = artistId;
        AlbumId = albumId;
        Genre = genre;
        AudioFile = audioFile;
        ReleaseDate = releaseDate;
    }
    
    public void Update(string title, Guid artistId, Guid? albumId, string genre, string audioFile, DateTime releaseDate)
    {
        Title = title;
        ArtistId = artistId;
        AlbumId = albumId;
        Genre = genre;
        AudioFile = audioFile;
        ReleaseDate = releaseDate;
    }
    
    public Song()
    {
    }
}