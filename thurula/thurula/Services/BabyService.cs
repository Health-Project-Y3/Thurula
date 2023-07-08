using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class BabyService: IBabyService
{
    private readonly IMongoCollection<Baby> _babies;
    public BabyService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babies = database.GetCollection<Baby>("babies");
    }
    public List<Baby> Get() =>
        _babies.Find(baby => true).ToList();
    public Baby Get(string id) =>
        _babies.Find<Baby>(baby => baby.Id == id).FirstOrDefault();
    public Baby Create(Baby baby)
    {
        _babies.InsertOne(baby);
        return baby;
    }
    public void Update(string id, Baby babyIn) =>
        _babies.ReplaceOne(baby => baby.Id == id, babyIn);
    public void Remove(Baby babyIn) =>
        _babies.DeleteOne(baby => baby.Id == babyIn.Id);
}