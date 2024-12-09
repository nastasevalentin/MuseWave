using FluentAssertions;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MuseWave.Domain.Entities;
using Xunit;

namespace MW.Domain.Core.Tests;

public class AlbumTests
{
    private readonly ServiceProvider _serviceProvider;

    public AlbumTests()
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
    public void When_CreateAlbumIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        var result = Album.Create("Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Album 1");
    }
    
    [Fact]
    public void When_CreateAlbumIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        var result = Album.Create(null, Guid.NewGuid(), "string", DateTime.Now, "string");
        
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_CreateAlbumIsCalled_And_GenreIsEmpty_Then_FailureIsReturned()
    {
        var result = Album.Create("Album 1", Guid.NewGuid(), null, DateTime.Now, "string");
        
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_UpdateAlbumIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        var albumResult = Album.Create("Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        var album = albumResult.Value;

        var result = Album.Update(album, "Album 1 Updated", Guid.NewGuid(), "string", DateTime.Now, "string");
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Album 1 Updated");
    }
    
    [Fact]
    public void When_UpdateAlbumIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        var albumResult = Album.Create("Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        var album = albumResult.Value;

        var result = Album.Update(album, null, Guid.NewGuid(), "string", DateTime.Now, "string");

        result.IsSuccess.Should().BeFalse();
    }
}
