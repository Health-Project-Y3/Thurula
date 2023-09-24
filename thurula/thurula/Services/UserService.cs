using System.Security.Claims;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMongoCollection<User> _users;
    private readonly IVaccineAppointmentService _vaccineAppointmentService;
    private readonly IBabyNameService _babyNames;
    private readonly IAuthUserService _authUserService;

    public UserService(IHttpContextAccessor httpContextAccessor, IAtlasDbSettings settings, IMongoClient client,
        IBabyNameService babyNames, IVaccineAppointmentService vaccineAppointmentService, IAuthUserService authUserService)
    {
        _httpContextAccessor = httpContextAccessor;
        _babyNames = babyNames;
        _vaccineAppointmentService = vaccineAppointmentService;
        _authUserService = authUserService;
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>("users");
    }

    public string GetMyName()
    {
        if (_httpContextAccessor.HttpContext is null) return "No user found";
        var result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        return result ?? "No user found";
    }

    public User GetByUsername(string username)
    {
        var user = _users.Find(user => user.Username == username).FirstOrDefault();
        if (!_authUserService.CheckAuth(userId: user.Id)) throw new UnauthorizedAccessException();
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return user;
    }

    public List<User> Get() =>
        _users.Find(user => true).ToList();

    public User Create(User user)
    {
        // Check if a user with the same username already exists
        var existingUser = _users.Find(u => u.Username == user.Username).FirstOrDefault();

        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with the same username already exists.");
        }
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.Gender = "female";
        user.DueVaccines = _vaccineAppointmentService.GetAllMotherVaccineIds();
        _users.InsertOne(user);
        return user;
    }

    public void Remove(User userIn) =>
        _users.DeleteOne(user => user.Id == userIn.Id);

    public User Get(string id)
    {
        var user = _users.Find(user => user.Id == id).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return user;
    }

    public void Update(string id, User user)
    {
        _users.ReplaceOne(user => user.Id == id, user);
    }

    public List<BabyNames> GetFavouriteNames(string id)
    {
        var user = _users.Find(user => user.Id == id).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        var favouriteNames = user.FavouriteNames;
        var babyNames = _babyNames.Get(favouriteNames);

        return babyNames;
    }

    public void AddFavouriteName(string id, string nameId)
    {
        var user = _users.Find(user => user.Id == id).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (user.FavouriteNames.Contains(nameId))
        {
            throw new Exception("Name already in favourites.");
        }

        user.FavouriteNames.Add(nameId);
        Update(id, user);
    }

    public void RemoveFavouriteName(string id, string nameId)
    {
        var user = _users.Find(user => user.Id == id).FirstOrDefault();
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        if (!user.FavouriteNames.Contains(nameId))
        {
            throw new Exception("Name not in favourites.");
        }

        user.FavouriteNames.Remove(nameId);
        Update(id, user);
    }

    /// <summary> Marks a vaccine appointment as completed or not completed </summary>
    /// <param name="userId"> The id of the user </param> <param name="vaccineId"> The id of the vaccine </param> <param name="mark">true to mark as completed, else false</param>
    public void MarkVaccineAppointment(string userId, string vaccineId, bool mark)
    {
        var user = Get(userId);
        if (user == null)
            throw new Exception("User not found");

        var vaccine = _vaccineAppointmentService.Get(vaccineId);
        if (vaccine == null)
            throw new Exception("Vaccine not found");
        else if (mark)
        {
            user.CompletedVaccines.Add(vaccineId);
            user.DueVaccines.Remove(vaccineId);
        } else
        {
            user.CompletedVaccines.Remove(vaccineId);
            user.DueVaccines.Add(vaccineId);
        }

        Update(userId, user);
    }

    public List<VaccineAppointments> GetDueVaccines(string userId)
    {
        var user = Get(userId);
        if (user == null)
            throw new Exception("User not found");
        var v = _vaccineAppointmentService.GetVaccines(user.DueVaccines);

        //calculate the days left for each appointment
        foreach (var vaccine in v)
        {
            var bornFor = (int)(DateTime.Now - user.ConceptionDate).TotalDays;
            vaccine.DaysFromBirth -= bornFor;
        }

        return v;
    }

    public List<VaccineAppointments> GetCompletedVaccines(string userId)
    {
        var user = Get(userId);
        if (user == null)
            throw new Exception("User not found");
        return _vaccineAppointmentService.GetVaccines(user.CompletedVaccines);
    }
}