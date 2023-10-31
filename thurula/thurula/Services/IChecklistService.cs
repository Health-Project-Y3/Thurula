using thurula.Models;

namespace thurula.Services;

public interface IChecklistService
{
    List<Checklist> Get();
    Checklist Get(string id);
    Checklist Create(Checklist checklist);
    // void Update(string id, Checklist babyIn);
    // void Remove(Checklist babyIn);
    List<Checklist> GetAllNewborns();

    List<Checklist> GetAllWeek2();
    List<Checklist> GetAllWeek3();
    List<Checklist> GetAllMonth1();
    List<Checklist> GetAllWeek5();
    List<Checklist> GetAllWeek6();
    List<Checklist> GetAllWeek7();
}