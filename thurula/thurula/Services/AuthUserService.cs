using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class AuthUserService : IAuthUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMongoCollection<Baby> _babies;
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _configuration;

    public AuthUserService(IHttpContextAccessor httpContextAccessor, IAtlasDbSettings settings, IMongoClient client,
        IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>("users");
        _babies = database.GetCollection<Baby>("babies");
    }

    public string GetMyName()
    {
        if (_httpContextAccessor.HttpContext is null) return "No user found";
        var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        return result ?? "No user found";
    }

    public bool CheckAuth(string babyId = "", string userId = "")
    {
        if (_httpContextAccessor.HttpContext is null) return false;
        //Allow admins to do anything
        var role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        if (role == "Admin") return true;
        // Get the user id from the token
        var tokenUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (tokenUserId == null) return false;
        // Check if the user is the owner of the baby
        if (babyId != "")
        {
            var baby = _babies.Find(baby => baby.Id == babyId).FirstOrDefault();
            return baby != null && baby.Owners.Contains(tokenUserId);
        }

        // Check if the user is the owner of the token
        if (userId != "")
        {
            return tokenUserId == userId;
        }

        return false;
    }

    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            user.Username == "admin" ? new Claim(ClaimTypes.Role, "Admin") : new Claim(ClaimTypes.Role, "User")
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

    public string Login(UserDto userDto)
    {
        var user = _users.Find(user => user.Username == userDto.Username).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
        {
            throw new Exception("Wrong password.");
        }

        var token = CreateToken(user);

        return token;
    }
}