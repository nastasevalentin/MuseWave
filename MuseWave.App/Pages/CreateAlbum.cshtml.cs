using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Domain.Entities;
using System.Threading.Tasks;
using Infrastructure;
using MuseWave.Identity;
using MuseWave.Identity.Models;

namespace MuseWave.App.Pages
{
    public class CreateAlbumModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GlobalMWContext _context;


        public CreateAlbumModel(UserManager<ApplicationUser> userManager, GlobalMWContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public Album Album { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Index");
            }

            if (Guid.TryParse(user.Id, out Guid artistId))
            {
                Album.ArtistId = artistId;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid user ID.");
                return Page();
            }

            // Add logic to save the album
            _context.Albums.Add(Album);
            await _context.SaveChangesAsync();

            return RedirectToPage("/User");
        }
    }
}