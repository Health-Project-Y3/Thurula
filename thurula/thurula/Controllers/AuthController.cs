using thurula.Models;
using thurula.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace thurula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthUserService _authUserService;
    private readonly IUserService _userService;

    public AuthController(IAuthUserService authUserService, IUserService userService)
    {
        _authUserService = authUserService;
        _userService = userService;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMyName()
    {
        return Ok(_authUserService.GetMyName());
    }

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {
        var user = new User
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = request.Password
        };
        try
        {
            _userService.Create(user);
        } catch (InvalidOperationException)
        {
            return Conflict("A user with the same username already exists.");
        } catch (Exception e)
        {
            return BadRequest(e);
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult Login(UserDto request)
    {
        try
        {
            var token = _authUserService.Login(request);
            return Ok(token);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("username/{username}")]
    public ActionResult<User> GetUser(string username)
    {
        try
        {
            var user = _userService.GetByUsername(username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(user);
        } catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}