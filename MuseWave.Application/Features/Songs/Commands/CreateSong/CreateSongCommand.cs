using MediatR;

namespace MuseWave.Application.Features.Songs.Commands.CreateSong
{
    public class CreateSongCommand : IRequest<CreateSongCommandResponse>
    {
        public string Title { get; set; } = default!;
        public Guid ArtistId { get; set; }
        public Guid? AlbumId { get; set; }
        public string Genre { get; set; } = default!;
        public string AudioFile { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
    }
}