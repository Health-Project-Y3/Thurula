using thurula.Models;

namespace thurula.Services;

public interface IUserService
{
    string GetMyName();
    
    List<User> Get();
    User Create(User user);
    void Remove(User userIn);
    User Get(string id);
    void Update(string id, User user);
    List<BabyNames> GetFavouriteNames(string id);
    void AddFavouriteName(string id, string name);
    void RemoveFavouriteName(string id, string name);
}