namespace MuseWave.Application.Features.Albums.Commands.CreateAlbum
{
    public class CreateAlbumDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid ArtistId { get; set; }
        public string Genre { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; } = default!;
    }
}