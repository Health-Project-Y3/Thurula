using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class BabyWeightChartService : IBabyWeightChartService
{
    private readonly IBabyService _babyService;
    private readonly IMongoCollection<BabyWeight> _babyWeights;

    public BabyWeightChartService(IAtlasDbSettings settings, IMongoClient client, IBabyService babyService)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babyWeights = database.GetCollection<BabyWeight>("baby_weights");
        _babyService = babyService;
    }

    /// <summary>given a baby id, month and weight, add the weight to the baby's weights list</summary>
    /// <param name="month">0-24 inclusive</param>
    /// <param name="weight">kg</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception> <exception cref="ArgumentException"></exception>
    public void AddWeight(string id, int month, double weight)
    {
        //validate data
        if (month is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 23.");
        }

        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be greater than 0.");
        }

        var baby = _babyService.Get(id);
        if (baby != null)
        {
            baby.Weights[month] = weight;
            _babyService.Update(id, baby);
        } else
        {
            throw new ArgumentException("Baby not found.");
        }
    }

    /// <summary> Delete the weight at the given month for the given baby </summary>
    /// <param name="month">0-24 inclusive</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception> <exception cref="ArgumentException"></exception>
    public void DeleteWeight(string id, int month)
    {
        if (month is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 23.");
        }

        var baby = _babyService.Get(id);
        if (baby != null)
        {
            baby.Weights[month] = -1.0;
            _babyService.Update(id, baby);
        } else
        {
            throw new ArgumentException("Baby not found.");
        }
    }

    /// <summary>    Edit the weight at the given month for the given baby </summary>
    /// <param name="month">0-24 inclusive</param> <param name="weight">kg</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception><exception cref="ArgumentException"></exception>
    public void EditWeight(string id, int month, double weight)
    {
        if (month is < 0 or > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 0 and 23.");
        }

        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be greater than 0.");
        }

        var baby = _babyService.Get(id);
        if (baby != null)
        {
            baby.Weights[month] = weight;
            _babyService.Update(id, baby);
        } else
        {
            throw new ArgumentException("Baby not found.");
        }
    }

    /// <summary> Given a baby id, return the list of weights for that baby </summary>
    /// <exception cref="ArgumentException"></exception>
    public List<double> GetWeight(string id)
    {
        var baby = _babyService.Get(id);
        if (baby is null)
        {
            throw new ArgumentException("Baby not found.");
        } else
        {
            return baby.Weights;
        }
    }

    /// <summary> gets all data points in a given percentile corresponding to months from 0-24 inclusive</summary>
    /// <param name="gender">male,female</param>
    /// <param name="percentile">10,25,50,75,90</param>
    /// <exception cref="NotImplementedException"></exception>
    public List<double> GetWeightReferenceData(string gender, int percentile)
    {
        var filter = Builders<BabyWeight>.Filter.And(
            Builders<BabyWeight>.Filter.Eq("Gender", gender),
            Builders<BabyWeight>.Filter.Eq("Percentile", percentile)
        );
        var result = _babyWeights.Find(filter).FirstOrDefault();
        if (result != null)
        {
            return result.Weights;
        } else
        {
            throw new ArgumentException("No data found.");
        }
    }
}