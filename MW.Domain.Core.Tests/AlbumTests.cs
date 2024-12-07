using FluentAssertions;
using MuseWave.Domain.Entities;

namespace MW.Domain.Core.Tests;

public class AlbumTests
{
    [Fact]
    public void When_CreateAlbumIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        // Arrange && Act
        var result = Album.Create("Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("Album 1");
    }
    
    [Fact]
    public void When_CreateAlbumIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        // Arrange && Act
        var result = Album.Create(null, Guid.NewGuid(), "string", DateTime.Now, "string");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_CreateAlbumIsCalled_And_GenreIsEmpty_Then_FailureIsReturned()
    {
        // Arrange && Act
        var result = Album.Create("Album 1", Guid.NewGuid(), null, DateTime.Now, "string");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_UpdateAlbumIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        // Arrange
        var albumResult = Album.Create("Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        var album = albumResult.Value;

        
        // Act
        var result = Album.Update(album, "Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void When_UpdateAlbumIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        // Arrange
        var albumResult = Album.Create("Album 1", Guid.NewGuid(), "string", DateTime.Now, "string");
        var album = albumResult.Value;

        
        // Act
        var result = Album.Update(album, null, Guid.NewGuid(), "string", DateTime.Now, "string");

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}