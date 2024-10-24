using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace MuseWave.App.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterC RegisterC { get; set; }
        
        public void OnGet()
        {
        }
        
        public void OnPost()
        {
           
        }
    }

    public class RegisterC
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
    }
}