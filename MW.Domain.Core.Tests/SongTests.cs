using FluentAssertions;
using MuseWave.Domain.Entities;

namespace MW.Domain.Core.Tests;

public class SongTests
{
    [Fact]
    public void When_CreateSongIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        // Arrange && Act
        var result = Song.Create("song 1", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Title.Should().Be("song 1");
    }
    
    [Fact]
    public void When_CreateSongIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        // Arrange && Act
        var result = Song.Create(null, Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_CreateSongIsCalled_And_ArtistIdIsEmpty_Then_FailureIsReturned()
    {
        // Arrange && Act
        var result = Song.Create("song 1", Guid.Empty, Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
    [Fact]
    public void When_UpdateSongIsCalled_And_TitleIsValid_And_ArtistIdIsValid_Then_SuccessIsReturned()
    {
        // Arrange
        var song = Song.Create("song 1", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now).Value;

        
        // Act
        var result = Song.Update(song, "song 2", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public void When_UpdateSongIsCalled_And_TitleIsNull_Then_FailureIsReturned()
    {
        // Arrange
        var song = Song.Create("song 1", Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now).Value;
        
        // Act
        var result = Song.Update(song, null, Guid.NewGuid(), Guid.NewGuid(), "string", "teest.mp3", DateTime.Now);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
    }
    
}