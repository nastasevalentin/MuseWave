using MediatR;
using MuseWave.Application.Features.Songs.Queries.GetAllSongs;

namespace MuseWave.Application.Features.Songs.Queries.GetAllSongs;

public record GetAllSongsQuery() : IRequest<GetAllSongsQueryResponse>;