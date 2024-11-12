using MediatR;

namespace MuseWave.Application.Features.Songs.Queries.GetAllSongsByArtistId;

public record GetAllSongsByArtistIdQuery(Guid ArtistId) : IRequest<GetAllSongsByArtistIdQueryResponse>;