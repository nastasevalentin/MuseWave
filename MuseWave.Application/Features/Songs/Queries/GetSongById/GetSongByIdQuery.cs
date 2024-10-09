namespace MuseWave.Application.Features.Songs.Queries.GetSongById;
using MediatR;
public record GetSongByIdQuery(Guid Id) : IRequest<SongDto?>;