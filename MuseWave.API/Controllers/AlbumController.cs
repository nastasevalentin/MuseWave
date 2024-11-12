using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuseWave.Application.Features.Albums.Commands.CreateAlbum;
using MuseWave.Application.Features.Albums.Commands.DeleteAlbum;
using MuseWave.Application.Features.Albums.Commands.UpdateAlbum;
using MuseWave.Application.Features.Albums.Queries.GetAlbumById;
using MuseWave.Application.Features.Albums.Queries.GetAllAlbums;
using MuseWave.Application.Features.Albums.Queries.GetAllAlbumsByArtistId;

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
        
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetAlbumByIdQuery(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteAlbumCommand(id));
            return Ok(result);
        }
        
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateAlbumCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }
        
        
        [HttpGet("Artist/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var result = await Mediator.Send(new GetAllAlbumsByArtistIdQuery(id));
            return Ok(result);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAlbums()
        {
            var result = await Mediator.Send(new GetAllAlbumsQuery());
            return Ok(result);
        }
    }
}