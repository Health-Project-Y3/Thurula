using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class NapService : INapService
{
    private readonly IMongoCollection<NapTimes> _babynaps;

    public NapService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babynaps = database.GetCollection<NapTimes>("baby_nap_tracking");
    }

    /// <summary> Gets a nap by id </summary>
    /// <param name="id">The identifier of the nap. </param>
    /// <returns>   The nap </returns>
    public NapTimes Get(string id)
    {
        var nap = _babynaps.Find(bn => bn.Id == id).FirstOrDefault();
        if (nap == null)
        {
            throw new Exception("Nap not found");
        }
        return nap;
    }

    /// <summary>   Gets all naps for a baby </summary>
    /// <param name="babyId">   The baby identifier. </param>
    /// <returns> A list of NapTimes </returns>
    public List<NapTimes> GetBabyNaps(string babyId)
    {
        var naps = _babynaps.Find(bn => bn.BabyId == babyId).SortBy(bn => bn.StartTime).ToList();
        return naps;
    }

    /// <summary>   Gets all naps for a baby within a given date range</summary>
    public List<NapTimes> GetBabyNapsRange(string babyId, DateTime start, DateTime end)
    {
        var naps = _babynaps.Find(bn => bn.BabyId == babyId && bn.StartTime >= start && bn.EndTime <= end).SortBy(bn => bn.StartTime).ToList();
        return naps;
    }

    /// <summary>   Creates a nap given all details </summary>
    public NapTimes Create(NapTimes napTime)
    {
        if (napTime.StartTime == DateTime.MinValue || napTime.EndTime == DateTime.MinValue)
        {
            throw new Exception("Start and End times must be set");
        }

        if (napTime.StartTime > napTime.EndTime)
        {
            throw new Exception("Start time must be before End time");
        }

        if (napTime.SleepQuality is < 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(napTime), "Sleep quality must be between 0 and 5");
        }

        _babynaps.InsertOne(napTime);
        return napTime;
    }

    /// <summary>   Updates the given nap by completely replacing </summary>
    public void Update(NapTimes napTimeIn)
    {
        var nap = _babynaps.Find(n => n.Id == napTimeIn.Id).FirstOrDefault();
        if (nap == null)
        {
            throw new Exception("Nap not found");
        }
        _babynaps.ReplaceOne(babyNap => babyNap.Id == napTimeIn.Id, napTimeIn);
    }

    public void Remove(NapTimes napTimeIn)
    {
        var nap = _babynaps.Find(n => n.Id == napTimeIn.Id).FirstOrDefault();
        if (nap == null)
        {
            throw new Exception("Nap not found");
        }
        _babynaps.DeleteOne(babyNap => babyNap.Id == napTimeIn.Id);
    }

    /// <summary>   Starts a nap by setting the start time to the current time </summary>
    /// <param name="loggedBy"> The baby owner who started the nap </param>
    /// <param name="babyId">   The baby identifier </param>
    /// <returns>The nap that was started</returns>
    public NapTimes StartNap(string babyId, string loggedBy = "")
    {
        var nap = new NapTimes
        {
            BabyId = babyId,
            StartTime = DateTime.Now,
            LoggedBy = loggedBy
        };
        _babynaps.InsertOne(nap);
        return nap;
    }

    /// <summary>   Ends a nap by setting the end time to the current time </summary>
    /// <returns>The nap that was ended</returns>
    public NapTimes EndNap(string napId)
    {
        var nap = _babynaps.Find(n => n.Id == napId).FirstOrDefault();
        if (nap == null)
        {
            throw new Exception("Nap not found");
        }

        nap.EndTime = DateTime.Now;
        Update(nap);
        return nap;
    }

    ///<summary>  Adds a rating for the given nap</summary>
    /// <param name="napId">The nap to rate</param>
    /// <param name="rating">The rating to add</param>
    ///<returns>The nap that was rated</returns>
    public NapTimes RateNap(string napId, int rating)
    {
        if (rating is < 0 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(rating), "Sleep quality must be between 0 and 5");
        }

        var nap = _babynaps.Find(n => n.Id == napId).FirstOrDefault();
        if (nap == null)
        {
            throw new Exception("Nap not found");
        }

        nap.SleepQuality = rating;
        Update(nap);
        return nap;
    }

    ///<summary>  Adds notes for the given nap</summary>
    /// <param name="napId">The nap to add notes to</param>
    /// <param name="notes">The notes to add</param>
    /// <returns>The nap that was updated</returns>
    public NapTimes AddSleepNotes(string napId, string notes)
    {
        var nap = _babynaps.Find(n => n.Id == napId).FirstOrDefault();
        if (nap == null)
        {
            throw new Exception("Nap not found");
        }

        nap.SleepNotes = notes;
        Update(nap);
        return nap;
    }
}