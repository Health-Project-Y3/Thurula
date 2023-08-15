using thurula.Models;

namespace thurula.Services;

public interface IBabyService
{
    List<Baby> Get();
    Baby Get(string id);
    Baby Create(Baby baby);
    void Update(string id, Baby babyIn);
    void Remove(Baby babyIn);
    void MarkVaccineAppointment(string babyId, string vaccineId, bool mark);
    List<VaccineAppointments> GetDueVaccines(string babyId);
    List<VaccineAppointments> GetCompletedVaccines(string babyId);
}