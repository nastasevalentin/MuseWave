using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseWave.Application.Features.Songs.Commands.CreateSong;
using MuseWave.Application.Features.Songs.Commands.UpdateSong;
using MuseWave.Application.Features.Songs.Queries;
using MuseWave.Application.Features.Songs.Queries.GetAllSongs;
using MuseWave.Application.Features.Songs.Queries.GetSongById;

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
        
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateSongCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetSongByIdQuery(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpGet("Album/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var result = await Mediator.Send(new GetAllSongsByAlbumIdQuery(id));
            return Ok(result);
        }
    }
}