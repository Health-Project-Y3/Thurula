using thurula.Models;

namespace thurula.Services;

public interface IUserService
{
    List<User> Get();
    User Get(string id);
    User Create(User user);
    void Update(string id, User userIn);
    void Remove(User userIn);
}