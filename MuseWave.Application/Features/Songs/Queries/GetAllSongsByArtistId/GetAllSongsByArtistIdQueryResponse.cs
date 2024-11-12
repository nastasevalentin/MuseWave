namespace MuseWave.Application.Features.Songs.Queries.GetAllSongsByArtistId;

public class GetAllSongsByArtistIdQueryResponse
{
    public GetAllSongsByArtistIdQueryResponse()
    {
        
    }
    public List<SongDto> Songs { get; set; }
}