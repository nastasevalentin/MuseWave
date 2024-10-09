using MuseWave.Application.Persistence;
using MediatR;

namespace MuseWave.Application.Features.Albums.Commands.DeleteAlbum;

public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, DeleteAlbumCommandResponse>
{
    private readonly IAlbumRepository repository;

    public DeleteAlbumCommandHandler(IAlbumRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<DeleteAlbumCommandResponse> Handle(DeleteAlbumCommand request,
        CancellationToken cancellationToken)
    {
        var album = await repository.GetByIdAsync(request.Id);
        if (!album.IsSuccess)
        {
            return new DeleteAlbumCommandResponse
            {
                Message = $"Song with id {request.Id} not found",
                Success = false
            };
        }

        await repository.DeleteAsync(request.Id);
        return new DeleteAlbumCommandResponse
        {
            Success = true
        };
    }
}