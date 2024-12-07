using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MuseWave.Domain.Entities;
using MuseWave.Identity.Models;

namespace MuseWave.App.Pages;

public class IndexModel : PageModel
{
    private readonly GlobalMWContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(GlobalMWContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<Album> RandomAlbums { get; set; }
    public List<Song> RandomSongs { get; set; }
    public List<ApplicationUser> RandomUsers { get; set; }


    public async Task OnGetAsync()
    {
        RandomAlbums = await _context.Albums.OrderBy(r => Guid.NewGuid()).Take(2).ToListAsync();
        RandomSongs = await _context.Songs.OrderBy(r => Guid.NewGuid()).Take(5).ToListAsync();
        
        var users = await _userManager.Users.ToListAsync();

        RandomUsers = users.Where(user => _userManager.IsInRoleAsync(user, "Artist").Result)
            .OrderBy(r => Guid.NewGuid())
            .Take(3)
            .ToList();
    }
}