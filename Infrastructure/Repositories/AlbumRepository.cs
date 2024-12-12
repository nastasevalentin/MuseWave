using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Common;
using MuseWave.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(GlobalMWContext context) : base(context)
        {
        }
        
        public async Task<Result<IReadOnlyList<Album>>> GetAllAlbumsByArtistId(Guid artistId)
        {
            var albums = await context.Albums.Where(a => a.ArtistId == artistId).ToListAsync();
            return Result<IReadOnlyList<Album>>.Success(albums);
        }
        
        public async Task<Result<IReadOnlyList<Album>>> GetAllAlbums()
        {
            var albums = await context.Albums.ToListAsync();
            return Result<IReadOnlyList<Album>>.Success(albums);
        }
    }
}