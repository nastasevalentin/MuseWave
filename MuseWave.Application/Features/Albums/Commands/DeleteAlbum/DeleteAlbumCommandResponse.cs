using MuseWave.Application.Responses;

namespace MuseWave.Application.Features.Albums.Commands.DeleteAlbum;

public class DeleteAlbumCommandResponse: BaseResponse
{
    public DeleteAlbumCommandResponse() : base()
    {
        
    }
     
    public AlbumDto Album { get; set; }
}