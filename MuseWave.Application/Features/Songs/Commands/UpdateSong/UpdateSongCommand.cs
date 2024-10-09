using MediatR;

namespace MuseWave.Application.Features.Songs.Commands.UpdateSong;

public class UpdateSongCommand: IRequest<UpdateSongCommandResponse>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid ArtistId { get; set; }
    public Guid? AlbumId { get; set; }
    public string Genre { get; set; }
    public string AudioFile { get; set; }
    public DateTime ReleaseDate { get; set; }
}