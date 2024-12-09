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
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MuseWave.Identity;

namespace MuseWave.App.Pages
{
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly HttpClient httpClient;
        private readonly GlobalMWContext mwContext;
        
        public ApplicationUser? appUser;
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Song> Songs { get; set; } = new List<Song>();
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsArtist { get; set; }
        public bool IsAdmin { get; set; }
        public List<ApplicationUser> ArtistRequests { get; set; } = new List<ApplicationUser>();

        public UserModel(UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory, GlobalMWContext mwContext)
        {
            this.userManager = userManager;
            this.httpClient = httpClientFactory.CreateClient(nameof(UserModel));
            this.mwContext = mwContext;
        }
        public async Task<IActionResult> OnPostDeleteSongAsync(string id)
        {
            if (Guid.TryParse(id, out Guid songId))
            {
                var song = await mwContext.Songs.FindAsync(songId);
                if (song != null)
                {
                    var user = await userManager.GetUserAsync(User);
                    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                    var isArtist = song.ArtistId == Guid.Parse(user.Id);

                    if (isAdmin || isArtist)
                    {
                        mwContext.Songs.Remove(song);
                        await mwContext.SaveChangesAsync();
                    }
                    else
                    {
                        return Forbid();
                    }
                }
            }
            return RedirectToPage();
        }
        
        public async Task<IActionResult> OnPostDeleteAlbumAsync(string id)
        {
            if (Guid.TryParse(id, out Guid albumId))
            {
                var album = await mwContext.Albums.FindAsync(albumId);
                if (album != null)
                {
                    var user = await userManager.GetUserAsync(User);
                    var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
                    var isArtist = album.ArtistId == Guid.Parse(user.Id);

                    if (isAdmin || isArtist)
                    {
                        mwContext.Albums.Remove(album);
                        await mwContext.SaveChangesAsync();
                    }
                    else
                    {
                        return Forbid();
                    }
                }
            }
            return RedirectToPage();
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
                    Songs = await FetchSongsForUser(appUser.Id.ToString());
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

        private async Task<List<Song>> FetchSongsForUser(string userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7165/api/v1/Songs/Artist/{userId}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var jsonObject = JsonNode.Parse(jsonResponse);
                    var songs = jsonObject["songs"].Deserialize<List<Song>>(options);
                    return songs ?? new List<Song>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching albums: {ex.Message}");
            }
            return new List<Song>();
        }
    }
}