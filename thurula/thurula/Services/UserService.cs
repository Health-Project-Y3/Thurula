using System.Security.Claims;

namespace thurula.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetMyName()
    {
        if (_httpContextAccessor.HttpContext is null) return "No user found";
        var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        return result ?? "No user found";
    }
}