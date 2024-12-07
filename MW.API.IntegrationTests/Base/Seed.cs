using Infrastructure;
using MuseWave.Domain.Entities;

namespace MW.API.IntegrationTests.Base;

public class Seed
{
    public static void InitializeDbForTests(GlobalMWContext context)
    {
        var songs = new List<Song>
        {
            Song.Create("Song 1", Guid.Parse("6d95df85-c2c8-4967-acf2-e470e85e5fa2"), Guid.Parse("6d95df85-c2c8-4967-acf2-e470e85e5fa2"), "string", "string", DateTime.Parse("2024-11-25")).Value,
            Song.Create("Song 1", Guid.Parse("aedefc40-7093-4118-abc3-b2f0929353fd"), Guid.Parse("6d95df85-c2c8-4967-acf2-e470e85e5fa2"), "string", "string", DateTime.Parse("2024-11-25")).Value,

        };
        
        var albums = new List<Album>
        {
            Album.Create("Album 1", Guid.Parse("6d95df85-c2c8-4967-acf2-e470e85e5fa2"), "string", DateTime.Parse("2024-11-25"), "string").Value,
            Album.Create("Album 2", Guid.Parse("aedefc40-7093-4118-abc3-b2f0929353fd"), "string", DateTime.Parse("2024-11-25"), "string").Value
        };
        
        
        context.Songs.RemoveRange();
        context.Songs.AddRange(songs);
        
        context.Albums.RemoveRange();
        context.Albums.AddRange(albums);
        
        context.SaveChanges();
        
       
        context.SaveChanges();
    } 
}