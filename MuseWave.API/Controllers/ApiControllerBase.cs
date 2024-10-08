

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MuseWave.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApiControllerBase: ControllerBase
    { 
        private ISender mediator = null!;
        
        protected ISender Mediator => mediator 
            ??= HttpContext.RequestServices
                .GetRequiredService<ISender>();
    }
}