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

        public async Task<IActionResult> OnGetAsync(string username)
        {
            Artist = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
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
    }
}