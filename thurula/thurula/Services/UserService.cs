using System.Security.Claims;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMongoCollection<User> _users;

    public UserService(IHttpContextAccessor httpContextAccessor, IAtlasDbSettings settings, IMongoClient client)
    {
        _httpContextAccessor = httpContextAccessor;
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>("users");
    }

    public string GetMyName()
    {
        if (_httpContextAccessor.HttpContext is null) return "No user found";
        var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        return result ?? "No user found";
    }
    
    public List<User> Get() =>
        _users.Find(user => true).ToList();
    public User Create(User user)
    {
        _users.InsertOne(user);
        return user;
    }
    public void Remove(User userIn) =>
        _users.DeleteOne(user => user.Id == userIn.Id);
}