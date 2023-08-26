using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class BabyNameService : IBabyNameService
{
    private readonly IMongoCollection<BabyNames> _babyNames;

    public BabyNameService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babyNames = database.GetCollection<BabyNames>("baby_names");
    }

    /// <summary> Get all baby names </summary>
    public List<BabyNames> Get() => _babyNames.Find(babyName => true).ToList();

    public BabyNames Get(string id) => _babyNames.Find(babyName => babyName.Id == id).FirstOrDefault();

    /// <summary> Get all baby names with the given ids </summary>
    public List<BabyNames> Get(IEnumerable<string> ids)
    {
        var filter = Builders<BabyNames>.Filter.In(babyName => babyName.Id, ids);
        return _babyNames.Find(filter).ToList();
    }

    public List<BabyNames> GetByGender(string gender) => _babyNames.Find(babyName => babyName.Gender == gender).ToList();


}