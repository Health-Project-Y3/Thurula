using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class DiaperService : IDiaperService
{
    private readonly IMongoCollection<DiaperTimes> _babydiapers;

    public DiaperService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _babydiapers = database.GetCollection<DiaperTimes>("baby_diaper_tracking");
    }

    /// <summary> Gets a diaper change by id </summary>
    public DiaperTimes Get(string id)
    {
        var diaper = _babydiapers.Find(bd => bd.Id == id).FirstOrDefault();
        if (diaper == null)
        {
            throw new Exception("Diaper change not found");
        }

        return diaper;
    }

    /// <summary>   Gets all diaper changes for a baby </summary>
    public List<DiaperTimes> GetBabyDiapers(string babyId)
    {
        var diapers = _babydiapers.Find(bd => bd.BabyId == babyId).SortBy(bd => bd.Time).ToList();
        return diapers;
    }

    /// <summary>   Creates a diaper change given all details </summary>
    public DiaperTimes Create(DiaperTimes diaper)
    {
        if (diaper.DiaperType != "wet" && diaper.DiaperType != "dirty" && diaper.DiaperType != "mixed" &&
            diaper.DiaperType != "neither")
        {
            throw new Exception("Invalid diaper type");
        }

        if (diaper.Time == DateTime.MinValue)
        {
            throw new Exception("Time must be set");
        }

        _babydiapers.InsertOne(diaper);
        return diaper;
    }

    /// <summary>   Updates the given diaper change by completely replacing </summary>
    public void Update(string id, DiaperTimes diaperIn)
    {
        if (diaperIn.DiaperType != "wet" && diaperIn.DiaperType != "dirty" && diaperIn.DiaperType != "mixed" &&
            diaperIn.DiaperType != "neither")
        {
            throw new Exception("Invalid diaper type");
        }

        if (diaperIn.Time == DateTime.MinValue)
        {
            throw new Exception("Time must be set");
        }

        var dp = Get(id);
        if (dp == null)
        {
            throw new Exception("Diaper change not found");
        }

        _babydiapers.ReplaceOne(diaper => diaper.Id == id, diaperIn);
    }

    /// <summary>   Removes the diaper change with the given id </summary>
    public void Remove(string id)
    {
        var dp = Get(id);
        if (dp == null)
        {
            throw new Exception("Diaper change not found");
        }

        _babydiapers.DeleteOne(diaper => diaper.Id == id);
    }
}