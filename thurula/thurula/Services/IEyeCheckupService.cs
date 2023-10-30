using thurula.Models;

namespace thurula.Services;

public interface IEyeCheckupService
{
    List<EyeCheckup> Get();
    List<EyeCheckup> GetEyeCheckup(string babyId);
    EyeCheckup Create(EyeCheckup eyecheckup);
}