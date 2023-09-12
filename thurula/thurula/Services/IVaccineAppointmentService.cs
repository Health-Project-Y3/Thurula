using thurula.Models;

namespace thurula.Services;

public interface IVaccineAppointmentService
{
    VaccineAppointments Get(string id);
    List<VaccineAppointments> GetVaccines(HashSet<string> vaccineIds);
    HashSet<string> GetAllBabyVaccineIds();
    HashSet<string> GetAllMotherVaccineIds();
}