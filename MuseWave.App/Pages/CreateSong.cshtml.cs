using System.ComponentModel.DataAnnotations;
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

        [BindProperty] public Song Song { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "The AudioFile field is required.")]
        public IFormFile? AudioFile { get; set; }

        [BindProperty(SupportsGet = true)] public Guid AlbumId { get; set; }

        public void OnGet()
        {
            // You can use AlbumId here if needed
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("CreateSong POST request received.");
            Console.WriteLine($"Audio file received: {AudioFile?.FileName} | legnth: {AudioFile?.Length}");
            
            
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid.");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }

                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("User is null.");
                return RedirectToPage("/Index");
            }

            if (AudioFile != null)
            {
                Console.WriteLine($"Audio file received: {AudioFile.FileName}");
                try
                {
                    var uploadsFolder = Path.Combine("wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Console.WriteLine("Uploads folder does not exist. Creating folder.");
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate a unique filename to avoid overwriting
                    var fileName = Path.GetFileNameWithoutExtension(AudioFile.FileName);
                    var extension = Path.GetExtension(AudioFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await AudioFile.CopyToAsync(stream);
                    }

                    // Store the relative file path in the Song entity
                    Song.AudioFile = Path.Combine("uploads", uniqueFileName);  // relative path to store in the DB
                    Console.WriteLine($"File saved to: {filePath}");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error uploading file: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while uploading the file.");
                    return Page();
                }
            }
            else
            {
                Console.WriteLine("No audio file received.");
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
                Console.WriteLine("Invalid user ID.");
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