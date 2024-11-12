using MuseWave.Application.Persistence;
using MuseWave.Domain.Common;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Persistence
{
    public interface IAlbumRepository : IAsyncRepository<Album>
    {
        Task<Result<IReadOnlyList<Album>>> GetAllAlbumsByArtistId(Guid artistId);
        Task<Result<IReadOnlyList<Album>>> GetAllAlbums();

    }
}