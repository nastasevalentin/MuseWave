using MuseWave.Application.Features.Songs;
using MuseWave.Application.Responses;

namespace MuseWave.Application.Features.Songs.Commands.UpdateSong;

public class UpdateSongCommandResponse: BaseResponse
{
    public UpdateSongCommandResponse(): base()
    {
    }
    
    public SongDto Song { get; set; }
}