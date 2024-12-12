using MuseWave.Domain.Common;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Services;

public class SongService
{
    public Result<Song> CreateSong(string title, Guid artistId, Guid? albumId, string genre, string audioFile, DateTime releaseDate)
    {
        var song = new Song(title, artistId, albumId, genre, audioFile, releaseDate);
        return Result<Song>.Success(song);
    }
    
    public Result<Song> UpdateSong(Song song, string title, Guid artistId, Guid? albumId, string genre, string audioFile, DateTime releaseDate)
    {
        song.Update(title, artistId, albumId, genre, audioFile, releaseDate);
        return Result<Song>.Success(song);
    }
}