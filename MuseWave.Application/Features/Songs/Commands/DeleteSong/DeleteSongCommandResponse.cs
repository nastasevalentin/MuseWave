using MuseWave.Application.Responses;

namespace MuseWave.Application.Features.Songs.Commands.DeleteSong;

public class DeleteSongCommandResponse: BaseResponse
{
    public DeleteSongCommandResponse() : base()
    {
        
    }
     
    public SongDto Song { get; set; }
}