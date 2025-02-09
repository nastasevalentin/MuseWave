using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MuseWave.API.Models;
using MuseWave.Application.Contracts.Identity;
using MuseWave.Application.Contracts.Interfaces;
using MuseWave.Application.Models.Identity;
using MuseWave.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace MuseWave.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly ICurrentUserService currentUserService;

    public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger,  ICurrentUserService currentUserService)
    {
        _authService = authService;
        _logger = logger;
        this.currentUserService = currentUserService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload");
            }

            var (status, message) = await _authService.Login(model);

            if (status == 0)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload");
            }
                    
            var (status, message) = await _authService.Registeration(model, UserRoles.User);

            if (status == 0)
            {
                return BadRequest(message);
            }

            return CreatedAtAction(nameof(Register), model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return Ok();
    }
    
    [HttpGet]
    [Route("currentuserinfo")]
    public CurrentUser CurrentUserInfo()
    {
        if (this.currentUserService.GetCurrentUserId() == null)
        {
            return new CurrentUser
            {
                IsAuthenticated = false
            };
        }
        return new CurrentUser
        {
            IsAuthenticated = true,
            UserName = this.currentUserService.GetCurrentUserId(),
            Claims = this.currentUserService.GetCurrentClaimsPrincipal().Claims.ToDictionary(c => c.Type, c => c.Value)
        };
    }
    
    
    [HttpGet("test-auth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return Ok("Token is valid!");
    }

}