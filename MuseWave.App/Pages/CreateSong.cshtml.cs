using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Domain.Entities;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using MuseWave.Identity;
using MuseWave.Identity.Models;

namespace MuseWave.App.Pages
{
    public class CreateSongModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GlobalMWContext _context;

        public CreateSongModel(UserManager<ApplicationUser> userManager, GlobalMWContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public Song Song { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid AlbumId { get; set; }

        public void OnGet()
        {
            // You can use AlbumId here if needed
        }

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
                Song.ArtistId = artistId;

                if (AlbumId != Guid.Empty)
                {
                    Song.AlbumId = AlbumId; // Set the AlbumId from the query string if it exists
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid user ID.");
                return Page();
            }

            // Print the request data
            Console.WriteLine($"Song Title: {Song.Title}");
            Console.WriteLine($"Song ArtistId: {Song.ArtistId}");
            Console.WriteLine($"Song AlbumId: {Song.AlbumId}");
            Console.WriteLine($"Song Genre: {Song.Genre}");
            Console.WriteLine($"Song Audio: {Song.AudioFile}");

            // Add logic to save the song
            _context.Songs.Add(Song);
            await _context.SaveChangesAsync();

            return RedirectToPage("/User");
        }
    }
}