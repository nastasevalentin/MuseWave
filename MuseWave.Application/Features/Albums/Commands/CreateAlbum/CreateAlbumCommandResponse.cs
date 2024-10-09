using MuseWave.Application.Responses;

namespace MuseWave.Application.Features.Albums.Commands.CreateAlbum
{
    public class CreateAlbumCommandResponse : BaseResponse
    {
        public CreateAlbumCommandResponse() : base()
        {
        }

        public AlbumDto Album { get; set; }
    }
}