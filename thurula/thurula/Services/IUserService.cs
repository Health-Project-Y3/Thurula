using thurula.Models;

namespace thurula.Services;

public interface IUserService
{
    string GetMyName();
    
    List<User> Get();
    User Create(User user);
    void Remove(User userIn);
    
}