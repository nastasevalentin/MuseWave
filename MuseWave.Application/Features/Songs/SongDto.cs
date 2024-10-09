namespace MuseWave.Application.Features.Songs
{
    public class SongDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid ArtistId { get; set; }
        public Guid? AlbumId { get; set; }
        public string Genre { get; set; } = default!;
        public string AudioFile { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
    }
}