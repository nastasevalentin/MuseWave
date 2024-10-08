using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseWave.Application.Features.Albums.Commands.CreateAlbum;

namespace MuseWave.API.Controllers
{
    public class AlbumsController : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(CreateAlbumCommand command)
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