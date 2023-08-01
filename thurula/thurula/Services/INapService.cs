using thurula.Models;

namespace thurula.Services;

public interface INapService
{
    NapTimes Get(string napId);
    List<NapTimes> GetBabyNaps(string babyId);
    NapTimes Create(NapTimes napTime);
    void Update(NapTimes napTimeIn);
    void Remove(NapTimes napTimeIn);
    NapTimes StartNap(string babyId, string loggedBy = "");
    NapTimes EndNap(string napId);
    NapTimes RateNap(string napId, int rating);
    NapTimes AddSleepNotes(string napId, string notes);

}