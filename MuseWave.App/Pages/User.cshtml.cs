using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Identity.Models;
using MuseWave.Domain.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MuseWave.App.Pages
{
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly HttpClient httpClient;

        public ApplicationUser? appUser;
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Song> Songs { get; set; } = new List<Song>();
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsArtist { get; set; }
        public bool IsAdmin { get; set; }
        public List<ApplicationUser> ArtistRequests { get; set; } = new List<ApplicationUser>();

        public UserModel(UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory)
        {
            this.userManager = userManager;
            this.httpClient = httpClientFactory.CreateClient(nameof(UserModel));
        }

        public async Task OnGetAsync()
        {
            var userName = User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                appUser = user;
                Roles = await userManager.GetRolesAsync(user);
                IsArtist = Roles.Contains("Artist");
                IsAdmin = Roles.Contains("Admin");
                if (IsArtist || IsAdmin)
                {
                    Albums = await FetchAlbumsForUser(appUser.Id.ToString());
                    Songs = FetchSongsForUser(appUser.Id.ToString());
                }
                if (IsAdmin)
                {
                    ArtistRequests = await userManager.Users.Where(u => u.HasRequestedRole).ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnPostApproveAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.HasRequestedRole = false;
                await userManager.AddToRoleAsync(user, "Artist");
                await userManager.UpdateAsync(user);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.HasRequestedRole = false;
                await userManager.UpdateAsync(user);
            }
            return RedirectToPage();
        }

        private async Task<List<Album>> FetchAlbumsForUser(string userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7165/api/v1/Albums/Artist/{userId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var jsonObject = JsonNode.Parse(jsonResponse);
                    var albums = jsonObject["albums"].Deserialize<List<Album>>(options);
                    return albums ?? new List<Album>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching albums: {ex.Message}");
            }
            return new List<Album>();
        }

        private List<Song> FetchSongsForUser(string userId)
        {
            return new List<Song>();
        }
    }
}