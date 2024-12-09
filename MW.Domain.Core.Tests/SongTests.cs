using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MuseWave.Domain.Entities;

namespace MW.Domain.Core.Tests;

public class SongTests
{
    private readonly ServiceProvider _serviceProvider;

    public SongTests()
    {
        var services = new ServiceCollection();
        
        services.RemoveAll<DbContextOptions>();
        services.RemoveAll(typeof(DbContextOptions<>));
        
        services.AddDbContext<GlobalMWContext>(options =>
        {
            options.UseInMemoryDatabase("GlobalMWDbForTesting");
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    private GlobalMWContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<GlobalMWContext>();
    }

    
    [Fact]
    public void When_CreateSongIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        var result = Song.Create("song 1", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("song 1");
    }
    
    [Fact]
    public void When_CreateSongIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        var result = Song.Create(null, Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_CreateSongIsCalled_And_ArtistIdIsEmpty_Then_FailureIsReturned()
    {
        var result = Song.Create("song 1", Guid.Empty, Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_UpdateSongIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        var song = Song.Create("song 1", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now).Value;

        
        var result = Song.Update(song, "song 2", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void When_UpdateSongIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        var song = Song.Create("song 1", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now).Value;
        
        var result = Song.Update(song, null, Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        result.IsSuccess.Should().BeFalse();
    }
    
}