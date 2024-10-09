using MuseWave.Application.Features.Songs.Queries.GetSongById;
using MediatR;
using MuseWave.Application.Features.Songs;
using MuseWave.Application.Persistence;


public class GetSongByIdHandler: IRequestHandler<GetSongByIdQuery, SongDto>
{
    private readonly ISongRepository repository;

    public GetSongByIdHandler(ISongRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<SongDto?> Handle(GetSongByIdQuery request, CancellationToken cancellationToken)
    {
        var song = await repository.GetByIdAsync(request.Id);
        return !song.IsSuccess ? null : new SongDto
        {
            Id = song.Value.Id,
            Title = song.Value.Title,
            ArtistId = song.Value.ArtistId,
            AlbumId = song.Value.AlbumId,
            Genre = song.Value.Genre,
            AudioFile = song.Value.AudioFile,
            ReleaseDate = song.Value.ReleaseDate
        };
    }

}