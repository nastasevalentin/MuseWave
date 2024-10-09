using MuseWave.Application.Features.Albums.Queries.GetAlbumById;
using MediatR;
using MuseWave.Application.Features.Albums;
using MuseWave.Application.Persistence;


public class GetAlbumByIdHandler: IRequestHandler<GetAlbumByIdQuery, AlbumDto>
{
    private readonly IAlbumRepository repository;

    public GetAlbumByIdHandler(IAlbumRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<AlbumDto?> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
    {
        var album = await repository.GetByIdAsync(request.Id);
        return !album.IsSuccess ? null : new AlbumDto
        {
            Id = album.Value.Id,
            Title = album.Value.Title,
            ArtistId = album.Value.ArtistId,
            Genre = album.Value.Genre,
            ReleaseDate = album.Value.ReleaseDate,
            CoverImage = album.Value.CoverImage
        };
    }

}