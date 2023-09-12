using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class UserExerciseService : IUserExerciseService
{
    private readonly IMongoCollection<UserExercise> _userExercise;

    public UserExerciseService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _userExercise = database.GetCollection<UserExercise>("user_exercise_records");
    }

    public List<UserExercise> Get() =>
        _userExercise.Find(uex => true).ToList();

    public UserExercise Get(string id) =>
        _userExercise.Find(uex => uex.Id == id).FirstOrDefault();

    public UserExercise Create(UserExercise uex)
    {
        _userExercise.InsertOne(uex);
        return uex;
    }

    public void Update(string id, UserExercise uex) =>
        _userExercise.ReplaceOne(u => u.Id == id, uex);

    public void Remove(string id) =>
        _userExercise.DeleteOne(u => u.Id == id);

    public List<UserExercise> GetByUser(string userId, DateTime? start, DateTime? end)
    {
        var filterBuilder = Builders<UserExercise>.Filter;
        var userIdFilter = filterBuilder.Eq(uex => uex.UserId, userId);

        if (start.HasValue && end.HasValue)
        {
            var dateFilter = filterBuilder.Gte(uex => uex.Date, start.Value) &
                             filterBuilder.Lte(uex => uex.Date, end.Value);

            var filter = userIdFilter & dateFilter;
            return _userExercise.Find(filter).ToList();
        }

        return _userExercise.Find(userIdFilter).ToList();
    }
}