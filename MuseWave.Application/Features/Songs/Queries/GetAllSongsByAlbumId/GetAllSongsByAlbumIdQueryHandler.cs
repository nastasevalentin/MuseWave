using MuseWave.Application.Features.Songs;
using MuseWave.Application.Features.Songs.Queries;
using MediatR;
using MuseWave.Application.Features.Songs.Queries.GetAllSongs;
using MuseWave.Application.Persistence;


public class GetAllSongsByAlbumIdQueryHandler: IRequestHandler<GetAllSongsByAlbumIdQuery, GetAllSongsByAlbumIdQueryResponse>
{
    private readonly ISongRepository repository;
    
    public GetAllSongsByAlbumIdQueryHandler(ISongRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<GetAllSongsByAlbumIdQueryResponse> Handle(GetAllSongsByAlbumIdQuery request, CancellationToken cancellationToken)
    {
        var songs = await repository.GetAllSongsByAlbumId(request.AlbumId);
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

        return new GetAllSongsByAlbumIdQueryResponse
        {
            Songs = songsDto
        };
    }
}