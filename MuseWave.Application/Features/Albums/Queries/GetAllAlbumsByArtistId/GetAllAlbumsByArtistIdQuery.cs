using MediatR;
using MuseWave.Application.Features.Albums.Queries.GetAllAlbumsByArtistId;

namespace MuseWave.Application.Features.Albums.Queries.GetAllAlbumsByArtistId;

public record GetAllAlbumsByArtistIdQuery(Guid ArtistId) : IRequest<GetAllAlbumsByArtistIdQueryResponse>;