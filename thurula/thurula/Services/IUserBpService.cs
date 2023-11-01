using thurula.Models;

namespace thurula.Services;

public interface IUserBpService
{
    List<UserBp> Get();
    UserBp Get(string id);
    UserBp Create(UserBp ubpr);
    List<UserBp> GetByUser(string userId, DateTime? start, DateTime? end);
    void Update(string id, UserBp ubpr);
    void Remove(string id);
    UserBp GetCurrentUserBp(string userId);

}