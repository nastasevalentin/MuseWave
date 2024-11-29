using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using MuseWave.Domain.Entities;
using MuseWave.Identity.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MuseWave.App.Pages
{
public class SearchModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly HttpClient httpClient;
    private readonly GlobalMWContext mwContext;

    public List<string> SearchResults { get; set; } = new List<string>();
    public string SelectedCategory { get; set; }
    public string Query { get; set; }
    public List<Album> Albums { get; set; } = new List<Album>();

    public SearchModel(UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory, GlobalMWContext mwContext)
    {
        this.userManager = userManager;
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        this.httpClient = new HttpClient(handler);
        this.mwContext = mwContext;

    }

    public async Task<IActionResult> OnPostSearchAsync(string query, string category)
    {
        Console.WriteLine("OnPostSearchAsync called with query: " + query + " and category: " + category);
        SearchResults.Clear();
        SelectedCategory = category;
        Query = query;
        if (!string.IsNullOrEmpty(query))
        {
            if (category == "Songs")
            {
                var songs = await FetchSongs();
                foreach (var song in songs.Where(s => s.Title.Contains(query, StringComparison.OrdinalIgnoreCase)))
                {
                    var album = await mwContext.Albums.FindAsync(song.AlbumId);
                    var albumTitle = album != null ? album.Title : "Unknown Album";
                    SearchResults.Add($"Song: {song.Title} | Audio: {song.AudioFile} | Album Id: {song.AlbumId} | Album Title: {albumTitle}");
                }
            }
            else if (category == "Albums")
            {
                var albums = await FetchAlbums();
                SearchResults.AddRange(albums
                    .Where(a => a.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Select(a => $"Album: {a.Title} | URL: /albums/{a.Title}"));
            }
            else if (category == "Artists")
            {
                var artists = await userManager.Users.ToListAsync();
                SearchResults.AddRange(artists
                    .Where(u => u.UserName.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Select(u => $"Artist: {u.UserName} | URL: /artists/{u.UserName}"));
            }
        }

        return Page();
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
    
    private async Task<List<Album>> FetchAlbums()
    {
        try
        {
            var response = await httpClient.GetAsync("https://localhost:7165/api/v1/Albums");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Albums Response: {jsonResponse}");
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

    private async Task<List<Song>> FetchSongs()
    {
        try
        {
            var response = await httpClient.GetAsync("https://localhost:7165/api/v1/Songs");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Songs API Response: {jsonResponse}");
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
            Console.WriteLine($"Error fetching songs: {ex.Message}");
        }

        return new List<Song>();
    }
}
}