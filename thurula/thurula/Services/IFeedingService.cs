using thurula.Models;

namespace thurula.Services;

public interface IFeedingService
{
    FeedingTimes Get(string id);
    List<FeedingTimes> GetBabyFeedings(string babyId);
    FeedingTimes Create(FeedingTimes feeding);
    void Update(string id, FeedingTimes feedingIn);
    void Remove(string id);
}