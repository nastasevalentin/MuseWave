using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Domain.Entities;
using System.IO;
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

        [BindProperty]
        [Required(ErrorMessage = "The CoverImage field is required.")]
        public IFormFile CoverImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("CreateAlbum POST request received.");
            // if (_context.Albums.Any(a => a.Title == Album.Title))
            // {
            //     ModelState.AddModelError("Album.Title", "A song with this title already exists.");
            //     return Page();
            // }
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

            if (Guid.TryParse(user.Id, out Guid artistId))
            {
                Album.ArtistId = artistId;
            }
            else
            {
                Console.WriteLine("Invalid user ID.");
                ModelState.AddModelError(string.Empty, "Invalid user ID.");
                return Page();
            }
            
            if (CoverImage != null)
            {
                Console.WriteLine($"Cover file received: {CoverImage.FileName}");
                try
                {
                    var uploadsFolder = Path.Combine("wwwroot", "covers");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Console.WriteLine("Covers folder does not exist. Creating folder.");
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Path.GetFileNameWithoutExtension(CoverImage.FileName);
                    var extension = Path.GetExtension(CoverImage.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }

                    Album.CoverImage = Path.Combine("covers", uniqueFileName);  // relative path to store in the DB
                    Console.WriteLine($"File saved to: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading file: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while uploading the file.");
                    return Page();
                }
            }
            else
            {
                Console.WriteLine("No audio file received.");
            }
            
            _context.Albums.Add(Album);
            await _context.SaveChangesAsync();

            Console.WriteLine("Album saved successfully.");
            return RedirectToPage("/User");
        }
    }
}