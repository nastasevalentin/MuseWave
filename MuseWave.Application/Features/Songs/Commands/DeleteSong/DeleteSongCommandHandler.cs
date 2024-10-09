using MuseWave.Application.Persistence;
using MediatR;

namespace MuseWave.Application.Features.Songs.Commands.DeleteSong;

public class DeleteSongCommandHandler: IRequestHandler<DeleteSongCommand, DeleteSongCommandResponse>
{
    private readonly ISongRepository repository;

    public DeleteSongCommandHandler(ISongRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<DeleteSongCommandResponse> Handle(DeleteSongCommand request,
        CancellationToken cancellationToken)
    {
        var song = await repository.GetByIdAsync(request.Id);
        if (!song.IsSuccess)
        {
            return new DeleteSongCommandResponse
            {
                Message = $"Song with id {request.Id} not found",
                Success = false
            };
        }

        await repository.DeleteAsync(request.Id);
        return new DeleteSongCommandResponse
        {
            Success = true
        };
    }
}