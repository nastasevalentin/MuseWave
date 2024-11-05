using Microsoft.AspNetCore.Identity;

namespace MuseWave.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public bool HasRequestedRole { get; set; } = false;
    }
}