using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Domain.Entities;
using MuseWave.Identity.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuseWave.Identity;

namespace MuseWave.App.Pages
{
    public class ArtistModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly GlobalMWContext dbContext;
        
        public ApplicationUser Artist { get; set; }
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Song> Songs { get; set; } = new List<Song>();

        public ArtistModel(UserManager<ApplicationUser> userManager, GlobalMWContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Artist = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (Artist == null)
            {
                return NotFound();
            }

            var artistId = new Guid(Artist.Id);

            Albums = await dbContext.Albums
                .Where(a => a.ArtistId == artistId)
                .ToListAsync<Album>();

            Songs = await dbContext.Songs
                .Where(s => s.ArtistId == artistId)
                .ToListAsync<Song>();

            return Page();
        }
        public async Task<IActionResult> OnPostDeleteSongAsync(string id)
        {
            if (Guid.TryParse(id, out Guid songId))
            {
                var song = await dbContext.Songs.FindAsync(songId);
                if (song != null)
                {
                    var user = await userManager.GetUserAsync(User);
                    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                    var isArtist = song.ArtistId == Guid.Parse(user.Id);

                    if (isAdmin || isArtist)
                    {
                        dbContext.Songs.Remove(song);
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        return Forbid();
                    }
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAlbumAsync(string id)
        {
            if (Guid.TryParse(id, out Guid albumId))
            {
                var album = await dbContext.Albums.FindAsync(albumId);
                if (album != null)
                {
                    var user = await userManager.GetUserAsync(User);
                    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                    var isArtist = album.ArtistId == Guid.Parse(user.Id);

                    if (isAdmin || isArtist)
                    {
                        dbContext.Albums.Remove(album);
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        return Forbid();
                    }
                }
            }
            return RedirectToPage();
        }
    }
}