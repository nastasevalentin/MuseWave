using Microsoft.AspNetCore.Identity;
using MuseWave.Identity.Models;
using System.Threading.Tasks;

namespace MuseWave.Identity.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> AssignRoleToUserAsync(ApplicationUser user, string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> RemoveRoleFromUserAsync(ApplicationUser user, string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                return result.Succeeded;
            }
            return false;
        }
    }
}