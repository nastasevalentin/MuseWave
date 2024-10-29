using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Domain.Entities;
using System.Threading.Tasks;

namespace MuseWave.App.Pages
{
    public class CreateSongModel : PageModel
    {
        [BindProperty]
        public Song Song { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add logic to save the song
            return RedirectToPage("/User");
        }
    }
}