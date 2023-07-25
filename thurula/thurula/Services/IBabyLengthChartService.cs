namespace thurula.Services;

public interface IBabyLengthChartService
{
    void AddLength(string id, int month, double length);
    void DeleteLength(string id, int month);
    void EditLength(string id, int month, double length);
    List<double> GetLengthReferenceData(string gender, int percentile);
    List<double> GetLength(string id);

}