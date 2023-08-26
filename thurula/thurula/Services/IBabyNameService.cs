using thurula.Models;

namespace thurula.Services;

public interface IBabyNameService
{
    List<BabyNames> Get();
    BabyNames Get(string id);
    List<BabyNames> Get(IEnumerable<string> ids);
    List<BabyNames> GetByGender(string gender);
}