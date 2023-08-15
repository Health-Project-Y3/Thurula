using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class FeedingService : IFeedingService
{
    private readonly IMongoCollection<FeedingTimes> _babyfeeding;

    public FeedingService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babyfeeding = database.GetCollection<FeedingTimes>("baby_feeding_tracking");
    }

    /// <summary> Gets a feeding time by id </summary>
    public FeedingTimes Get(string id)
    {
        var feeding = _babyfeeding.Find(bd => bd.Id == id).FirstOrDefault();
        if (feeding == null)
        {
            throw new Exception("Feeding time not found");
        }

        return feeding;
    }

    /// <summary>   Gets all feeding times for a baby </summary>
    public List<FeedingTimes> GetBabyFeedings(string babyId)
    {
        var feedings = _babyfeeding.Find(bd => bd.BabyId == babyId).SortBy(bd => bd.StartTime).ToList();
        return feedings;
    }

    /// <summary>   Creates a feeding time given all details </summary>
    public FeedingTimes Create(FeedingTimes feeding)
    {
        if (feeding.FeedingType != "liquid" && feeding.FeedingType != "solid" && feeding.FeedingType != "leftbreast" &&
            feeding.FeedingType != "rightbreast")
        {
            throw new Exception("Invalid feeding type");
        }

        if (feeding.StartTime == DateTime.MinValue)
        {
            throw new Exception("Start time must be set");
        }

        if (feeding.EndTime == DateTime.MinValue)
        {
            throw new Exception("End time must be set");
        }
        if (feeding.EndTime < feeding.StartTime)
        {
            throw new Exception("End time must be after start time");
        }

        if (feeding.FeedingType is "liquid" or "leftbreast" or "rightbreast" && feeding.FeedingAmount.Unit is not ("ml" or ""))
        {
            throw new Exception("Invalid units for liquid feeding");
        }
        if (feeding.FeedingType is "solid" && (feeding.FeedingAmount.Unit != "g" && feeding.FeedingAmount.Unit != ""))
        {
            throw new Exception("Invalid units for solid feeding");
        }

        _babyfeeding.InsertOne(feeding);
        return feeding;
    }


    /// <summary>   Updates the given feeding time by completely replacing </summary>
    public void Update(string id, FeedingTimes feedingIn)
    {
        if (feedingIn.FeedingType != "liquid" && feedingIn.FeedingType != "solid" && feedingIn.FeedingType != "leftbreast" &&
            feedingIn.FeedingType != "rightbreast")
        {
            throw new Exception("Invalid feeding type");
        }

        if (feedingIn.StartTime == DateTime.MinValue)
        {
            throw new Exception("Start time must be set");
        }

        if (feedingIn.EndTime == DateTime.MinValue)
        {
            throw new Exception("End time must be set");
        }

        var fd = Get(id);
        if (fd == null)
        {
            throw new Exception("Feeding time not found");
        }

        _babyfeeding.ReplaceOne(feeding => feeding.Id == id, feedingIn);
    }


    /// <summary>   Removes the feeding time with the given id </summary>
    public void Remove(string id)
    {
        var fd = Get(id);
        if (fd == null)
        {
            throw new Exception("Feeding time not found");
        }

        _babyfeeding.DeleteOne(feeding => feeding.Id == id);
    }

}