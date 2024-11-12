using MuseWave.Application.Features.Albums;
using MuseWave.Application.Features.Albums.Queries.GetAllAlbums;
using MediatR;
using MuseWave.Application.Persistence;

public class GetAllAlbumsQueryHandler : IRequestHandler<GetAllAlbumsQuery, GetAllAlbumsQueryResponse>
{
    private readonly IAlbumRepository repository;

    public GetAllAlbumsQueryHandler(IAlbumRepository repository)
    {
        this.repository = repository;
    }

    public async Task<GetAllAlbumsQueryResponse> Handle(GetAllAlbumsQuery request, CancellationToken cancellationToken)
    {
        var albums = await repository.GetAllAlbums();
        var albumsDto = albums.Value.Select(album => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ArtistId = album.ArtistId,
            Genre = album.Genre,
            ReleaseDate = album.ReleaseDate,
            CoverImage = album.CoverImage
        }).ToList();

        return new GetAllAlbumsQueryResponse
        {
            Albums = albumsDto
        };
    }
}