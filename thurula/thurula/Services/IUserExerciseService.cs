using thurula.Models;

namespace thurula.Services;

public interface IUserExerciseService
{
    List<UserExercise> Get();
    UserExercise Get(string id);
    UserExercise Create(UserExercise uex);
    List<UserExercise> GetByUser(string userId, DateTime? start, DateTime? end);
    void Update(string id, UserExercise uex);
    void Remove(string id);
}