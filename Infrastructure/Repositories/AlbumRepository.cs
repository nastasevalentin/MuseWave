using Infrastructure;
using Infrastructure.Repositories;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(GlobalMWContext context) : base(context)
        {
        }
    }
}