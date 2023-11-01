using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class UserWeightService : IUserWeightService
{
    private readonly IMongoCollection<UserWeight> _userWeight;

    public UserWeightService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _userWeight = database.GetCollection<UserWeight>("user_weight_records");
    }

    public List<UserWeight> Get() =>
        _userWeight.Find(uwr => true).ToList();

    public UserWeight Get(string id) =>
        _userWeight.Find(uwr => uwr.Id == id).FirstOrDefault();

    public UserWeight Create(UserWeight uwr)
    {
        _userWeight.InsertOne(uwr);
        return uwr;
    }

    public void Update(string id, UserWeight uwr) =>
        _userWeight.ReplaceOne(u => u.Id == id, uwr);

    public void Remove(string id) =>
        _userWeight.DeleteOne(u => u.Id == id);

    public List<UserWeight> GetByUser(string userId, DateTime? start, DateTime? end)
    {
        var filterBuilder = Builders<UserWeight>.Filter;
        var userIdFilter = filterBuilder.Eq(uwr => uwr.UserId, userId);

        if (start.HasValue && end.HasValue)
        {
            var dateFilter = filterBuilder.Gte(uwr => uwr.Date, start.Value) &
                             filterBuilder.Lte(uwr => uwr.Date, end.Value);

            var filter = userIdFilter & dateFilter;
            return _userWeight.Find(filter).ToList();
        }

        return _userWeight.Find(userIdFilter).ToList();
    }
}