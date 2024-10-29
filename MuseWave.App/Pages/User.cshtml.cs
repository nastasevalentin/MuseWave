using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Identity.Models;
using MuseWave.Domain.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MuseWave.App.Pages
{
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly HttpClient httpClient; // Add HttpClient

        public ApplicationUser? appUser;
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Song> Songs { get; set; } = new List<Song>();
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsArtist { get; set; }
        public bool IsAdmin { get; set; }

        public UserModel(UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory) // Inject HttpClient
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
                Console.WriteLine("ID" + appUser.Id);
                Roles = await userManager.GetRolesAsync(user);
                IsArtist = Roles.Contains("Artist");
                IsAdmin = Roles.Contains("Admin");
                if (IsArtist || IsAdmin)
                {
                    Albums = await FetchAlbumsForUser(appUser.Id.ToString());
                    Songs = FetchSongsForUser(appUser.Id.ToString());
                }
            }
        }

        private async Task<List<Album>> FetchAlbumsForUser(string userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7165/api/v1/Albums/Artist/{userId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON Response: " + jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var jsonObject = JsonNode.Parse(jsonResponse);
                    var albumsNode = jsonObject?["albums"];
                    var albums = albumsNode?.Deserialize<List<Album>>(options);

                    return albums ?? new List<Album>();
                }
                else
                {
                    Console.WriteLine($"Error fetching albums: {response.StatusCode}, Content: {jsonResponse}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception fetching albums: {ex.Message}");
            }

            return new List<Album>();
        }

        private List<Song> FetchSongsForUser(string userId)
        {
            // Replace with actual data fetching logic
            return new List<Song> { };
        }
    }
}