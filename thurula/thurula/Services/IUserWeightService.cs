using thurula.Models;

namespace thurula.Services;

public interface IUserWeightService
{
    List<UserWeight> Get();
    UserWeight Get(string id);
    UserWeight Create(UserWeight uwr);
    List<UserWeight> GetByUser(string userId, DateTime? start, DateTime? end);
    void Update(string id, UserWeight uwr);
    void Remove(string id);
}