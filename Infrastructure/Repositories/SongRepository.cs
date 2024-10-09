using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Common;
using MuseWave.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        public SongRepository(GlobalMWContext context) : base(context)
        {
        }
        
        public virtual async Task<Result<IReadOnlyList<Song>>> GetAllSongsByAlbumId(Guid albumId)
        {
            var songs = await context.Songs.Where(s => s.AlbumId == albumId).ToListAsync();
            return Result<IReadOnlyList<Song>>.Success(songs);
        }
    }
}