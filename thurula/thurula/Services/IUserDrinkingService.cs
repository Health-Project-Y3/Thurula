using thurula.Models;

namespace thurula.Services;

public interface IUserDrinkingService
{
    List<UserDrinking> Get();
    UserDrinking Get(string id);
    UserDrinking Create(UserDrinking udr);
    List<UserDrinking> GetByUser(string userId, DateTime? start, DateTime? end);
    void Update(string id, UserDrinking udr);
    void Remove(string id);
}