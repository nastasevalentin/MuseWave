using MuseWave.Application.Features.Songs;
using MuseWave.Application.Features.Songs.Queries;
using MediatR;
using MuseWave.Application.Features.Songs.Queries.GetAllSongs;
using MuseWave.Application.Features.Songs.Queries.GetAllSongsByArtistId;
using MuseWave.Application.Persistence;


public class GetAllSongsByArtistIdQueryHandler: IRequestHandler<GetAllSongsByArtistIdQuery, GetAllSongsByArtistIdQueryResponse>
{
    private readonly ISongRepository repository;
    
    public GetAllSongsByArtistIdQueryHandler(ISongRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<GetAllSongsByArtistIdQueryResponse> Handle(GetAllSongsByArtistIdQuery request, CancellationToken cancellationToken)
    {
        var songs = await repository.GetAllSongsByArtistId(request.ArtistId);
        var songsDto = songs.Value.Select(song => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ArtistId = song.ArtistId,
            AlbumId = song.AlbumId,
            Genre = song.Genre,
            AudioFile = song.AudioFile,
            ReleaseDate = song.ReleaseDate
        }).ToList();

        return new GetAllSongsByArtistIdQueryResponse
        {
            Songs = songsDto
        };
    }
}