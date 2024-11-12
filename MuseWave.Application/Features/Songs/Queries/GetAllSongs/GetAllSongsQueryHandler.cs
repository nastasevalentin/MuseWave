using MuseWave.Application.Features.Songs;
using MuseWave.Application.Features.Songs.Queries;
using MediatR;
using MuseWave.Application.Features.Songs.Queries.GetAllSongs;
using MuseWave.Application.Persistence;

public class GetAllSongsQueryHandler : IRequestHandler<GetAllSongsQuery, GetAllSongsQueryResponse>
{
    private readonly ISongRepository repository;

    public GetAllSongsQueryHandler(ISongRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetAllSongsQueryResponse> Handle(GetAllSongsQuery request, CancellationToken cancellationToken)
    {
        var songs = await repository.GetAllSongs();
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

        return new GetAllSongsQueryResponse
        {
            Songs = songsDto
        };
    }
}