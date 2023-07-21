using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class BabyChartService : IBabyChartService
{
    private readonly IBabyService _babyService;

    public BabyChartService(IAtlasDbSettings settings, IMongoClient client, IBabyService babyService)
    {
        _babyService = babyService;
    }

    /// <summary>given a baby id, month and length, add the length to the baby's lengths list</summary>
    /// <param name="month">0-24 inclusive</param>
    /// <param name="length">cm</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception> <exception cref="ArgumentException"></exception>
    public void AddLength(string id, int month, double length)
    {
        //validate data
        if (month is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 23.");
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than 0.");
        }

        var baby = _babyService.Get(id);
        if (baby != null)
        {
            baby.Lengths[month] = length;
            _babyService.Update(id, baby);

        } else
        {
            throw new ArgumentException("Baby not found.");
        }
    }

    /// <summary> Delete the length at the given month for the given baby </summary>
    /// <param name="month">0-24 inclusive</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception> <exception cref="ArgumentException"></exception>
    public void DeleteLength(string id, int month)
    {
        if (month is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 23.");
        }

        var baby = _babyService.Get(id);
        if (baby != null)
        {
            baby.Lengths[month] = -1.0;
            _babyService.Update(id, baby);
        } else
        {
            throw new ArgumentException("Baby not found.");
        }
    }

    /// <summary>    Edit the length at the given month for the given baby </summary>
    /// <param name="month">0-24 inclusive</param> <param name="length">cm</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception><exception cref="ArgumentException"></exception>
    public void EditLength(string id, int month, double length)
    {
        if (month is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 23.");
        }

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than 0.");
        }

        var baby = _babyService.Get(id);
        if (baby != null)
        {
            baby.Lengths[month] = length;
            _babyService.Update(id, baby);
        } else
        {
            throw new ArgumentException("Baby not found.");
        }
    }
}