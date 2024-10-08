using MuseWave.Application.Responses;

namespace MuseWave.Application.Features.Songs.Commands.CreateSong
{
    public class CreateSongCommandResponse : BaseResponse
    {
        public CreateSongCommandResponse() : base()
        {
        }

        public CreateSongDto Song { get; set; }
    }
}