using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using thurula.Models;
using thurula.Services;

namespace thurula.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public static User user = new User();
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMyName()
    {
        return Ok(_userService.GetMyName());
        // var isAuthorized = User?.Identity?.IsAuthenticated;

        // var userName = User?.Identity?.Name;
        //var roleClaims = User?.FindAll(ClaimTypes.Role);
        //var roles = roleClaims?.Select(c => c.Value).ToList();
        //var roles2 = User?.Claims
        //    .Where(c => c.Type == ClaimTypes.Role)
        //    .Select(c => c.Value)
        //    .ToList();
        // return Ok(new { User.Identity.Name });
    }

    [HttpPost("register")]
    public ActionResult<User> Register(UserDto request)
    {
        //check if user exists
        var users = _userService.Get();
        if (users.Any(u => u.Username == request.Username))
        {
            return BadRequest("Username already exists.");
        }
        
        string passwordHash
            = BCrypt.Net.BCrypt.HashPassword(request.Password);

        user.Username = request.Username;
        user.PasswordHash = passwordHash;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        
        _userService.Create(user);

        return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult<User> Login(UserDto request)
    {
        var users = _userService.Get();
        user = users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null)
        {
            return BadRequest("User not found.");
        }
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return BadRequest("Wrong password.");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User userIn)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userIn.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}