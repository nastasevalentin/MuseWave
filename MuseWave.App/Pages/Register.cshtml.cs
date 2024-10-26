using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Identity.Models;

namespace MuseWave.App.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public RegisterC RegisterC { get; set; }
        
        public string SuccessMessage { get; set; }


        public RegisterModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName = RegisterC.Username,
                Name = RegisterC.Name,
                Email = RegisterC.Email
            };

            var result = await _userManager.CreateAsync(user, RegisterC.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
                SuccessMessage = "Your account has been created successfully!";
                return Page();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }

    public class RegisterC
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}