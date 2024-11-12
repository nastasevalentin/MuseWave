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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MuseWave.App.Pages
{
public class SearchModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly HttpClient httpClient;

    public List<string> SearchResults { get; set; } = new List<string>();
    public string SelectedCategory { get; set; }
    public string Query { get; set; }

    public SearchModel(UserManager<ApplicationUser> userManager, IHttpClientFactory httpClientFactory)
    {
        this.userManager = userManager;
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        this.httpClient = new HttpClient(handler);
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
                SearchResults.AddRange(songs
                    .Where(s => s.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Select(s => $"Song: {s.Title}"));
            }
            else if (category == "Albums")
            {
                var albums = await FetchAlbums();
                SearchResults.AddRange(albums
                    .Where(a => a.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Select(a => $"Album: {a.Title}"));
            }
            else if (category == "Artists")
            {
                var artists = await userManager.Users.ToListAsync();
                SearchResults.AddRange(artists
                    .Where(u => u.UserName.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .Select(u => $"Artist: {u.UserName}"));
            }
        }

        return Page();
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