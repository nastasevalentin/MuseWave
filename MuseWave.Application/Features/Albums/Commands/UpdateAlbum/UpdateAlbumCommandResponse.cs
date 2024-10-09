using MuseWave.Application.Features.Albums;
using MuseWave.Application.Responses;

namespace MuseWave.Application.Features.Albums.Commands.UpdateAlbum
{
    public class UpdateAlbumCommandResponse: BaseResponse
    {
        public UpdateAlbumCommandResponse(): base()
        {
        }
        
        public AlbumDto Album { get; set; }
    }
}
