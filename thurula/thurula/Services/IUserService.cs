using thurula.Models;

namespace thurula.Services;

public interface IUserService
{
    string GetMyName();

    List<User> Get();
    User GetByUsername(string username);
    User Create(User user);
    void Remove(User userIn);
    User Get(string id);
    void Update(string id, User user);
    List<BabyNames> GetFavouriteNames(string id);
    void AddFavouriteName(string id, string name);
    void RemoveFavouriteName(string id, string name);
    void MarkVaccineAppointment(string userId, string vaccineId, bool mark);
     List<VaccineAppointments> GetDueVaccines(string userId);
     List<VaccineAppointments> GetCompletedVaccines(string userId);

}