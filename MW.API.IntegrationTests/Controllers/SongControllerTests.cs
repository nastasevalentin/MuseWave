using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using MW.API.IntegrationTests.Base;
using MuseWave.Application.Features.Songs;
using MuseWave.Application.Features.Songs.Commands.CreateSong;
using MuseWave.Application.Features.Songs.Commands.UpdateSong;
using Newtonsoft.Json;

namespace MW.API.IntegrationTests.Controllers;

public class SongControllerTests: BaseApplicationContextTests
{
    private const string RequestUri = "/api/v1/Songs";

    [Fact]
    public async Task When_GetAllSongsQueryHandlerIsCalled_Then_Success()
    {
        // Arrange && Act
        string token = CreateToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await Client.GetAsync(RequestUri);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorResponse}");
        }

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SongWrapper>(responseString);
    
        // Assert
        result?.Songs.Count.Should().Be(2);
    }
    
    [Fact]
    public async Task When_PostSongCommandHandlerIsCalledWithRightParameters_Then_TheEntityCreatedShouldBeReturned()
    {
        // Arrange
        string token = new JwtTokenBuilder()
            .WithRole("Artist")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var song = new CreateSongCommand()
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("0603a85b-01ed-403c-afd3-917bb353bca6"),
            AlbumId = Guid.Parse("400cc8d7-34dd-45d8-a601-2322ef110082"),
            Genre = "string",
            AudioFile = "string",
            ReleaseDate = DateTime.Parse("2022-01-01"),
        };
        // Act
        var response = await Client.PostAsJsonAsync(RequestUri, song);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateSongCommandResponse>(responseString);
        result?.Should().NotBeNull();
        result?.Song.Title.Should().Be(song.Title);
    }
    
    [Fact]
    public async Task When_PostSongCommandHandlerIsCalledWithWrongParameters_Then_400ShouldBeReturned()
    {
        // Arrange
        string token = new JwtTokenBuilder()
            .WithRole("Admin")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var course = new CreateSongCommand
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("98523166-585a-4aee-8d7e-d7af07663e3d"),
            AlbumId = Guid.Parse("98523166-585a-4aee-8d7e-d7af07663e3d"),
            Genre = "string",
            AudioFile = "string",
        };
        // Act
        var response = await Client.PostAsJsonAsync(RequestUri, course);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task When_PostSongCommandHandlerIsCalledWithWrongParameters_Then_401ShouldBeReturned()
    {
        // Arrange
        var course = new CreateSongCommand
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("3b65d7b3-8f4a-41d8-81f3-587ebe783a65"),
            AlbumId = Guid.Parse("fe34959b-fd71-44f1-bf0d-75479ebc5755"),
            Genre = "string",
            AudioFile = "string",
            ReleaseDate = DateTime.Parse("2022-01-01"),
        };
        // Act
        var response = await Client.PostAsJsonAsync(RequestUri, course);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task When_GetSongByIdQueryHandlerIsCalledWithRightParameters_Then_Success()
    {
        // Arrange
        string token = new JwtTokenBuilder()
            .WithRole("Admin")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var song = new CreateSongCommand()
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("f5c48577-dac7-4782-9097-5715f0378fbf"),
            AlbumId = Guid.Parse("2d97a463-4227-4687-b7ad-8f0ccc65bf87"),
            Genre = "string",
            AudioFile = "string",
            ReleaseDate = DateTime.Parse("2022-01-01"),
        };
    
        var createResponse = await Client.PostAsJsonAsync(RequestUri, song);
        createResponse.EnsureSuccessStatusCode();
        var createResponseString = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonConvert.DeserializeObject<CreateSongCommandResponse>(createResponseString);

        if (createResult == null || createResult.Song == null)
        {
            throw new InvalidOperationException("Failed to create song.");
        }

        // Act
        var response = await Client.GetAsync($"{RequestUri}/{createResult.Song.Id}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<SongDto>(responseString);

        // Assert
        result?.Title.Should().Be(createResult.Song.Title);
    }
    
    [Fact]
    public async Task When_GetSongByIdQueryHandlerIsCalledWrongRightParameters_Then_404ShouldBeReturned()
    {
        // Arrange
        string token = CreateToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // Act
        var response = await Client.GetAsync($"{RequestUri}/{Guid.NewGuid()}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task When_UpdateSongCommandHandlerIsCalledWithRightParameters_Then_Success()
    {
        // Arrange
        string token = new JwtTokenBuilder()
            .WithRole("Admin")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var course = new CreateSongCommand
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("504c59d0-cdc7-4f68-9a64-8c9c829ad5b5"),
            AlbumId = Guid.Parse("dd1a0375-50db-41d1-8b54-be213b611d35"),
            Genre = "string",
            AudioFile = "string",
            ReleaseDate = DateTime.Parse("2022-01-01"),
        };
    
        var createResponse = await Client.PostAsJsonAsync(RequestUri, course);
        createResponse.EnsureSuccessStatusCode();
        var createResponseString = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonConvert.DeserializeObject<CreateSongCommandResponse>(createResponseString);

        if (createResult == null || createResult.Song == null)
        {
            throw new InvalidOperationException("Failed to create song.");
        }

        var updateSong = new UpdateSongCommand()
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("c6b2b5ef-a433-4617-8292-0519a22b20e0"),
            AlbumId = Guid.Parse("7572a529-19ec-497f-aec9-2ca464bca85a"),
            Genre = "TEST",
            AudioFile = "string",
            ReleaseDate = DateTime.Parse("2022-01-01"),
        };
    
        // Act
        var response = await Client.PutAsJsonAsync($"{RequestUri}/{createResult.Song.Id}", updateSong);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<UpdateSongCommandResponse>(responseString);
    
        // Assert
        result?.Song.Title.Should().Be(updateSong.Title);
    }
    
    [Fact]
    public async Task When_DeleteSongCommandHandlerIsCalledWithAdminRole_Then_Success()
    {
        // Arrange
        string token = new JwtTokenBuilder()
            .WithRole("Admin")
            .Build();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var song = new CreateSongCommand()
        {
            Title = "Test Song",
            ArtistId = Guid.Parse("e778994d-a33c-409a-b24c-6cec2ca4662c"),
            AlbumId = Guid.Parse("fb2a48de-30d1-4c8f-8188-3226e088816c"),
            Genre = "string",
            AudioFile = "string",
            ReleaseDate = DateTime.Parse("2022-01-01"),
        };

        var createResponse = await Client.PostAsJsonAsync(RequestUri, song);
        var createResponseString = await createResponse.Content.ReadAsStringAsync();
        var createResult = JsonConvert.DeserializeObject<CreateSongCommandResponse>(createResponseString);

        // Act
        var response = await Client.DeleteAsync($"{RequestUri}/{createResult.Song.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task When_DeleteSongCommandHandlerIsCalledWithoutArtistOrAdminRole_Then_401ShouldBeReturned()
    {
        // Arrange && Act
        var response = await Client.DeleteAsync($"{RequestUri}/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private string CreateToken()
    {
        return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
            new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                new List<Claim> { new(ClaimTypes.Role, "User"), new("department", "Security") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: JwtTokenProvider.SigningCredentials
            ));
    }
}

public class SongWrapper
{
    public List<SongDto> Songs { get; set; }
}
