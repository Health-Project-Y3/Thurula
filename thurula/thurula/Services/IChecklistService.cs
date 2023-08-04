using thurula.Models;

namespace thurula.Services;

public interface IChecklistService
{
    List<Checklist> Get();
    Checklist Get(string id);
    Checklist Create(Checklist checklist);
    // void Update(string id, Checklist babyIn);
    // void Remove(Checklist babyIn);
}