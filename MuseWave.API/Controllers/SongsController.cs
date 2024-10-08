using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseWave.Application.Features.Songs.Commands.CreateSong;

namespace MuseWave.API.Controllers
{
    public class SongsController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateSongCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}