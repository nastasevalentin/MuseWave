using MediatR;

namespace MuseWave.Application.Features.Songs.Commands.DeleteSong;

public record DeleteSongCommand(Guid Id): IRequest<DeleteSongCommandResponse>;