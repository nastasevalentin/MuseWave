using MediatR;

namespace MuseWave.Application.Features.Albums.Commands.DeleteAlbum;

public record DeleteAlbumCommand(Guid Id): IRequest<DeleteAlbumCommandResponse>;