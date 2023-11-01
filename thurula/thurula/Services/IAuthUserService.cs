using thurula.Models;

namespace thurula.Services;

public interface IAuthUserService
{
    string GetMyName();
    bool CheckAuth(string babyId= "", string userId = "");
    string CreateToken(User user);
    string Login(UserDto user);
}