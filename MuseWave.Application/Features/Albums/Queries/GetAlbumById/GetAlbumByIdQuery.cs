namespace MuseWave.Application.Features.Albums.Queries.GetAlbumById;
using MediatR;
public record GetAlbumByIdQuery(Guid Id) : IRequest<AlbumDto?>;