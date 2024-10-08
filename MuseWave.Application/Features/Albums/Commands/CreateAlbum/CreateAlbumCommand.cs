using MediatR;

namespace MuseWave.Application.Features.Albums.Commands.CreateAlbum
{
    public class CreateAlbumCommand : IRequest<CreateAlbumCommandResponse>
    {
        public string Title { get; set; } = default!;
        public Guid ArtistId { get; set; }
        public string Genre { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; } = default!;
    }
}