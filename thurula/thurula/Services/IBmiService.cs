using thurula.Models;

namespace thurula.Services;

public interface IBmiService
{
    double CalculateBmi(double height, double weight);
    string CheckRange(User user);


}