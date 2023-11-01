using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using thurula.Models;

namespace thurula.Services;

public class EyeCheckupService : IEyeCheckupService
{
    private readonly IMongoCollection<EyeCheckup> _eyecheckup;

    public EyeCheckupService(IAtlasDbSettings settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.DatabaseName);
        _eyecheckup = database.GetCollection<EyeCheckup>("eye_checkup");
    }

    public List<EyeCheckup> Get() =>
        _eyecheckup.Find(checkup => true).ToList();


    public List<EyeCheckup> GetEyeCheckup(string babyId)
    {
        var checkups = _eyecheckup.Find<EyeCheckup>(bd => bd.BabyId == babyId).SortBy(bd => bd.CheckedDate).ToList();
        return checkups;
    }

    /// <summary>   Creates a feeding time given all details </summary>
    public EyeCheckup Create(EyeCheckup eyecheckup)
    {
        _eyecheckup.InsertOne(eyecheckup);
        return eyecheckup;
    }


   

}