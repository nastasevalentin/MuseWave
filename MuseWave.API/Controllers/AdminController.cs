using Microsoft.AspNetCore.Mvc;
using MuseWave.Identity.Models;
using MuseWave.Identity.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MuseWave.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(RoleService roleService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleService = roleService;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser(string userName, string roleName)
        {

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await _roleService.AssignRoleToUserAsync(user, roleName);
                if (result)
                {
                    return Ok("Role assigned successfully");
                }

                return BadRequest("Failed to assign role");
            }

            return NotFound("User not found");
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUser(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _roleService.RemoveRoleFromUserAsync(user, roleName);
            if (result)
            {
                return Ok("Role removed successfully");
            }

            return BadRequest("Failed to remove role");
        }
        
        [HttpGet("get-user-roles")]
        public async Task<IActionResult> GetUserRoles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"User '{userName}' not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count == 0)
            {
                return Ok($"User '{userName}' has no assigned roles.");
            }

            return Ok(new { User = userName, Roles = roles });
        }
    }
}