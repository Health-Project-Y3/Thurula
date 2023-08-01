using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class BabyLengthChartService : IBabyLengthChartService
{
    private readonly IBabyService _babyService;
    private readonly IMongoCollection<BabyLength> _babyLengths;

    public BabyLengthChartService(IAtlasDbSettings settings, IMongoClient client, IBabyService babyService)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babyLengths = database.GetCollection<BabyLength>("baby_lengths");
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

    /// <summary> Given a baby id, return the list of lengths for that baby </summary>
    /// <exception cref="ArgumentException"></exception>
    public List<double> GetLength(string id)
    {
        var baby = _babyService.Get(id);
        if (baby is null)
        {
            throw new ArgumentException("Baby not found.");
        } else
        {
            return baby.Lengths;
        }
    }

    /// <summary> gets all data points in a given percentile corresponding to months from 0-24 inclusive</summary>
    /// <param name="gender">male,female</param>
    /// <param name="percentile">5,10,15,25,50,75,90</param>
    /// <exception cref="NotImplementedException"></exception>
    public List<double> GetLengthReferenceData(string gender, int percentile)
    {
        var filter = Builders<BabyLength>.Filter.And(
            Builders<BabyLength>.Filter.Eq("Gender", gender),
            Builders<BabyLength>.Filter.Eq("Percentile", percentile)
        );
        var result = _babyLengths.Find(filter).FirstOrDefault();
        if (result != null)
        {
            return result.Lengths;
        } else
        {
            throw new ArgumentException("No data found.");
        }
    }
}