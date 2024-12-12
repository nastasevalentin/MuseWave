using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace MuseWave.App.Pages;

public class AlbumPage : PageModel
{
    public IReadOnlyList<Song> Songs { get; private set; }

    private readonly IAlbumRepository _albumRepository;
    private readonly ISongRepository _songRepository;
    public AlbumPage(IAlbumRepository albumRepository, ISongRepository songRepository)
    {
        _albumRepository = albumRepository;
        _songRepository = songRepository;
    }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }
    

    public Album Album { get; private set; }

    public async Task OnGetAsync()
    {
        var result = await _albumRepository.GetByIdAsync(Id);
        if (result.IsSuccess)
        {
            Album = result.Value;
            var songsResult = await _songRepository.GetAllSongsByAlbumId(Id);
            if (songsResult.IsSuccess)
            {
                Songs = songsResult.Value;
            }
        }
    }
}