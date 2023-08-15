using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class VaccineAppointmentService : IVaccineAppointmentService
{
    private readonly IMongoCollection<VaccineAppointments> _vaccineAppointments;

    public VaccineAppointmentService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _vaccineAppointments = database.GetCollection<VaccineAppointments>("vaccine_appointments");
    }

    /// <summary> Gets a vaccine appointment by id </summary>
    public VaccineAppointments Get(string id)
    {
        var vaccine = _vaccineAppointments.Find(v => v.Id == id).FirstOrDefault();
        if (vaccine == null)
        {
            throw new Exception("Vaccine appointment not found");
        }

        return vaccine;
    }

    /// <summary> Gets all the vaccines for a given set of ids </summary>
    public List<VaccineAppointments> GetVaccines(HashSet<string> vaccineIds)
    {
        var vaccines = _vaccineAppointments.Find(v => vaccineIds.Contains(v.Id)).SortBy(v => v.DaysFromBirth).ToList();
        return vaccines;
    }

    /// <summary> Gets all the ids of all vaccine appointments </summary>
    public HashSet<string> GetAllIds()
    {
        var vaccines = _vaccineAppointments.Find(v => true).ToList();
        var ids = new HashSet<string>();
        foreach (var vaccine in vaccines)
        {
            ids.Add(vaccine.Id);
        }

        return ids;
    }
}