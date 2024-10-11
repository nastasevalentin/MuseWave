using MuseWave.Application.Features.Albums;
using MuseWave.Application.Features.Songs.Queries;
using MediatR;
using MuseWave.Application.Features.Albums.Queries.GetAllAlbumsByArtistId;
using MuseWave.Application.Persistence;


public class GetAllAlbumsByArtistIdQueryHandler: IRequestHandler<GetAllAlbumsByArtistIdQuery, GetAllAlbumsByArtistIdQueryResponse>
{
    private readonly IAlbumRepository repository;
    
    public GetAllAlbumsByArtistIdQueryHandler(IAlbumRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<GetAllAlbumsByArtistIdQueryResponse> Handle(GetAllAlbumsByArtistIdQuery request, CancellationToken cancellationToken)
    {
        var albums = await repository.GetAllAlbumsByArtistId(request.ArtistId);
        var albumsDto = albums.Value.Select(song => new AlbumDto
        {
            Id = song.Id,
            Title = song.Title,
            ArtistId = song.ArtistId,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            CoverImage = song.CoverImage
        }).ToList();

        return new GetAllAlbumsByArtistIdQueryResponse
        {
            Albums = albumsDto
        };
    }
}