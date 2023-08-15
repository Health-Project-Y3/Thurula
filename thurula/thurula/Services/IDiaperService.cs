using thurula.Models;

namespace thurula.Services;

public interface IDiaperService
{
    DiaperTimes Get(string id);
    List<DiaperTimes> GetBabyDiapers(string babyId);
    DiaperTimes Create(DiaperTimes diaper);
    void Update(string id, DiaperTimes diaperIn);
    void Remove(string id);
    
}