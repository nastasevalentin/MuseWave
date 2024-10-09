using MuseWave.Domain.Entities;
using MediatR;
using MuseWave.Application.Persistence;

namespace MuseWave.Application.Features.Songs.Commands.UpdateSong;

public class UpdateSongCommandHandler: IRequestHandler<UpdateSongCommand, UpdateSongCommandResponse>
{
    private readonly ISongRepository repository;

    public UpdateSongCommandHandler(ISongRepository repository)
    {
        this.repository = repository;
    }

    public async Task<UpdateSongCommandResponse> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        var song = await repository.GetByIdAsync(request.Id);
        if (!song.IsSuccess)
        {
            return new UpdateSongCommandResponse
            {
                Message = $"Song with id {request.Id} not found",
                Success = false
            };
        }
        
        var updatedSong = Song.Update(song.Value, request.Title, request.ArtistId, request.AlbumId, request.Genre, request.AudioFile, request.ReleaseDate);
        if (!song.IsSuccess)
        {
            return new UpdateSongCommandResponse
            {
                Message = updatedSong.Error,
                Success = false
            };
        }

        await repository.UpdateAsync(updatedSong.Value);
        
        return new UpdateSongCommandResponse
        {
            Success = true,
            Song = new SongDto
            {
                Id = updatedSong.Value.Id,
                Title = updatedSong.Value.Title,
                ArtistId = updatedSong.Value.ArtistId,
                AlbumId = updatedSong.Value.AlbumId,
                Genre = updatedSong.Value.Genre,
                AudioFile = updatedSong.Value.AudioFile,
                ReleaseDate = updatedSong.Value.ReleaseDate
            }
        };
    }
}