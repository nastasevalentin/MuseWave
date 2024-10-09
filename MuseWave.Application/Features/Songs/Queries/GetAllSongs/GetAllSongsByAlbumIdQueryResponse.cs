namespace MuseWave.Application.Features.Songs.Queries.GetAllSongs;

public class GetAllSongsByAlbumIdQueryResponse
{
    public GetAllSongsByAlbumIdQueryResponse()
    {
        
    }
    public List<SongDto> Songs { get; set; }
}