using MuseWave.Domain.Common;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Services;

public class AlbumService
{
    public Result<Album> CreateAlbum(string title, Guid artistId, string genre, DateTime releaseDate, string coverImage)
    {
        var album = new Album(title, artistId, genre, releaseDate, coverImage);
        return Result<Album>.Success(album);
    }
    
    public Result<Album> UpdateAlbum(Album album, string title, Guid artistId, string genre, DateTime releaseDate, string coverImage)
    {
        album.Update(title, artistId, genre, releaseDate, coverImage);
        return Result<Album>.Success(album);
    }
}