using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MuseWave.App.Pages
{
    public class RequestArtistRoleModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RequestArtistRoleModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                user.HasRequestedRole = true;
                await userManager.UpdateAsync(user);
                return RedirectToPage("/User");
            }
            return Page();
        }
    }
}