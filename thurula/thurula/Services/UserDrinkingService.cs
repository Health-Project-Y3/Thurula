using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class UserDrinkingService : IUserDrinkingService
{
    private readonly IMongoCollection<UserDrinking> _userDrinking;

    public UserDrinkingService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _userDrinking = database.GetCollection<UserDrinking>("user_drinking_records");
    }

    public List<UserDrinking> Get() =>
        _userDrinking.Find(udr => true).ToList();

    public UserDrinking Get(string id) =>
        _userDrinking.Find(udr => udr.Id == id).FirstOrDefault();

    public UserDrinking Create(UserDrinking udr)
    {
        _userDrinking.InsertOne(udr);
        return udr;
    }

    public void Update(string id, UserDrinking udr) =>
        _userDrinking.ReplaceOne(u => u.Id == id, udr);

    public void Remove(string id) =>
        _userDrinking.DeleteOne(u => u.Id == id);

    public List<UserDrinking> GetByUser(string userId, DateTime? start, DateTime? end)
    {
        var filterBuilder = Builders<UserDrinking>.Filter;
        var userIdFilter = filterBuilder.Eq(udr => udr.UserId, userId);

        if (start.HasValue && end.HasValue)
        {
            var dateFilter = filterBuilder.Gte(udr => udr.Date, start.Value) &
                             filterBuilder.Lte(udr => udr.Date, end.Value);

            var filter = userIdFilter & dateFilter;
            return _userDrinking.Find(filter).ToList();
        }

        return _userDrinking.Find(userIdFilter).ToList();
    }
}