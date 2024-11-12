using MediatR;

namespace MuseWave.Application.Features.Albums.Queries.GetAllAlbums;

public record GetAllAlbumsQuery() : IRequest<GetAllAlbumsQueryResponse>;