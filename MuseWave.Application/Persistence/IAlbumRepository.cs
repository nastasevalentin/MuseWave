using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Persistence
{
    public interface IAlbumRepository : IAsyncRepository<Album>
    {
    }
}