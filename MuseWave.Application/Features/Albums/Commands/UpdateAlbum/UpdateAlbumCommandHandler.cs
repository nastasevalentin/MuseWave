using MuseWave.Domain.Entities;
using MediatR;
using MuseWave.Application.Persistence;
using MuseWave.Application.Services;

namespace MuseWave.Application.Features.Albums.Commands.UpdateAlbum;

public class UpdateAlbumCommandHandler: IRequestHandler<UpdateAlbumCommand, UpdateAlbumCommandResponse>
{
    private readonly IAlbumRepository repository;
    private readonly AlbumService albumService;


    public UpdateAlbumCommandHandler(IAlbumRepository repository, AlbumService albumService)
    {
        this.repository = repository;
        this.albumService = albumService;
    }

    public async Task<UpdateAlbumCommandResponse> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var album = await repository.GetByIdAsync(request.Id);
        if (!album.IsSuccess)
        {
            return new UpdateAlbumCommandResponse
            {
                Message = $"Album with id {request.Id} not found",
                Success = false
            };
        }

        var updatedAlbum = albumService.UpdateAlbum(album.Value, request.Title, request.ArtistId, request.Genre, request.ReleaseDate, request.CoverImage);
        if (!album.IsSuccess)
        {
            return new UpdateAlbumCommandResponse
            {
                Message = updatedAlbum.Error,
                Success = false
            };
        }

        await repository.UpdateAsync(updatedAlbum.Value);
        
        return new UpdateAlbumCommandResponse
        {
            Success = true,
            Album = new AlbumDto
            {
                Id = album.Value.Id,
                Title = album.Value.Title,
                ArtistId = album.Value.ArtistId,
                Genre = album.Value.Genre,
                ReleaseDate = album.Value.ReleaseDate,
                CoverImage = album.Value.CoverImage
            }
        };
    }
}