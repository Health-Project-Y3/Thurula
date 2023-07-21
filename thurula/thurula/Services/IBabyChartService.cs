namespace thurula.Services;

public interface IBabyChartService
{
    void AddLength(string id, int month, double length);
    void DeleteLength(string id, int month);
    void EditLength(string id, int month, double length);

}