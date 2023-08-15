using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class ChecklistService: IChecklistService
{
    private readonly IMongoCollection<Checklist> _checklists;

    public ChecklistService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _checklists = database.GetCollection<Checklist>("checklists");
    }

    public List<Checklist> Get() =>
        _checklists.Find(checklist => true).ToList();

    public Checklist Get(string id) =>
        _checklists.Find<Checklist>(checklist => checklist.Id == id).FirstOrDefault();


    public List<Checklist> GetAllNewborns()
    {
        List<Checklist>  check = _checklists.Find<Checklist>(checklist => checklist.Period == "Newborn").ToList();
        // Console.WriteLine(check);
        check.ForEach(Console. WriteLine);
        return _checklists.Find<Checklist>(checklist => checklist.Period == "Newborn").ToList();
    }

    public Checklist Create(Checklist checklist)
    {
        _checklists.InsertOne(checklist);
        return checklist;
    }
    // public void Update(string id, Baby babyIn) =>
    //     _babies.ReplaceOne(baby => baby.Id == id, babyIn);
    // public void Remove(Baby babyIn) =>
    //     _babies.DeleteOne(baby => baby.Id == babyIn.Id);
}