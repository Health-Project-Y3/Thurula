using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class UserBpService : IUserBpService
{
    private readonly IMongoCollection<UserBp> _userBp;

    public UserBpService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _userBp = database.GetCollection<UserBp>("user_bp_records");
    }

    public List<UserBp> Get() =>
        _userBp.Find(ubpr => true).ToList();

    public UserBp Get(string id) =>
        _userBp.Find(ubpr => ubpr.Id == id).FirstOrDefault();

    public UserBp Create(UserBp ubpr)
    {
        _userBp.InsertOne(ubpr);
        return ubpr;
    }

    public void Update(string id, UserBp ubpr) =>
        _userBp.ReplaceOne(u => u.Id == id, ubpr);

    public void Remove(string id) =>
        _userBp.DeleteOne(u => u.Id == id);

    public List<UserBp> GetByUser(string userId, DateTime? start, DateTime? end)
    {
        var filterBuilder = Builders<UserBp>.Filter;
        var userIdFilter = filterBuilder.Eq(ubpr => ubpr.UserId, userId);

        if (start.HasValue && end.HasValue)
        {
            var dateFilter = filterBuilder.Gte(ubpr => ubpr.Date, start.Value) &
                             filterBuilder.Lte(ubpr => ubpr.Date, end.Value);

            var filter = userIdFilter & dateFilter;
            return _userBp.Find(filter).ToList();
        }

        return _userBp.Find(userIdFilter).ToList();
    }

    public UserBp GetCurrentUserBp(string userId)
    {
        var filterBuilder = Builders<UserBp>.Filter;
        var userIdFilter = filterBuilder.Eq(ubpr => ubpr.UserId, userId);
        return _userBp.Find(userIdFilter).SortByDescending(ubpr => ubpr.Date).FirstOrDefault();
    }
}