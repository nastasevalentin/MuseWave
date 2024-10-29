using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace MuseWave.App.Pages;

public class AlbumPage : PageModel
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumPage(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }

    [BindProperty(SupportsGet = true)]
    public string Title { get; set; }

    public Album Album { get; private set; }

    public async Task OnGetAsync()
    {
        var albums = await _albumRepository.GetAll();
        var album = albums.Value.FirstOrDefault(a => a.Title.Equals(Title, StringComparison.OrdinalIgnoreCase));
        if (album != null)
        {
            var result = await _albumRepository.GetByIdAsync(album.Id);
            if (result.IsSuccess)
            {
                Album = result.Value;
            }
        }
    }
}