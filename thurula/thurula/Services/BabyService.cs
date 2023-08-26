using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class BabyService : IBabyService
{
    private readonly IMongoCollection<Baby> _babies;
    private readonly IVaccineAppointmentService _vaccineAppointmentService;

    public BabyService(IAtlasDbSettings settings, IMongoClient client,
        IVaccineAppointmentService vaccineAppointmentService)
    {
        _vaccineAppointmentService = vaccineAppointmentService;
        var database = client.GetDatabase(settings.DatabaseName);
        _babies = database.GetCollection<Baby>("babies");
    }

    public List<Baby> Get() =>
        _babies.Find(baby => true).ToList();

    public Baby Get(string id) =>
        _babies.Find(baby => baby.Id == id).FirstOrDefault();

    public Baby Create(Baby baby)
    {
        baby.DueVaccines = _vaccineAppointmentService.GetAllIds();
        _babies.InsertOne(baby);
        return baby;
    }

    public void Update(string id, Baby babyIn) =>
        _babies.ReplaceOne(baby => baby.Id == id, babyIn);

    public void Remove(Baby babyIn) =>
        _babies.DeleteOne(baby => baby.Id == babyIn.Id);

    public List<Baby> GetByParentId(string id) =>
        _babies.Find(baby => baby.Owners.Contains(id)).ToList();

    /// <summary> Marks a vaccine appointment as completed or not completed </summary>
    /// <param name="babyId"> The id of the baby </param> <param name="vaccineId"> The id of the vaccine </param> <param name="mark">true to mark as completed, else false</param>
    public void MarkVaccineAppointment(string babyId, string vaccineId, bool mark)
    {
        var baby = Get(babyId);
        if (baby == null)
            throw new Exception("Baby not found");

        var vaccine = _vaccineAppointmentService.Get(vaccineId);
        if (vaccine == null)
            throw new Exception("Vaccine not found");
        else if (mark)
        {
            baby.CompletedVaccines.Add(vaccineId);
            baby.DueVaccines.Remove(vaccineId);
        } else
        {
            baby.CompletedVaccines.Remove(vaccineId);
            baby.DueVaccines.Add(vaccineId);
        }

        Update(babyId, baby);
    }

    public List<VaccineAppointments> GetDueVaccines(string babyId)
    {
        var baby = Get(babyId);
        if (baby == null)
            throw new Exception("Baby not found");
        var v =  _vaccineAppointmentService.GetVaccines(baby.DueVaccines);

        //calculate the days left for each appointment
        foreach (var vaccine in v)
        {
            var bornFor = (int) (DateTime.Now - baby.BirthDate).TotalDays;
            vaccine.DaysFromBirth -= bornFor;
        }
        return v;
    }

    public List<VaccineAppointments> GetCompletedVaccines(string babyId)
    {
        var baby = Get(babyId);
        if (baby == null)
            throw new Exception("Baby not found");
        return _vaccineAppointmentService.GetVaccines(baby.CompletedVaccines);
    }
}