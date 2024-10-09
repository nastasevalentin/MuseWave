using MuseWave.Application.Persistence;
using MuseWave.Domain.Common;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Persistence
{
    public interface ISongRepository : IAsyncRepository<Song>
    {
        Task<Result<IReadOnlyList<Song>>> GetAllSongsByAlbumId(Guid albumId);
    }
}