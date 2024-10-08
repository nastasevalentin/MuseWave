using Infrastructure;
using Infrastructure.Repositories;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        public SongRepository(GlobalMWContext context) : base(context)
        {
        }
    }
}