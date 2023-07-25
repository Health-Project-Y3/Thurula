namespace thurula.Services;

public interface IBabyWeightChartService
{
    void AddWeight(string id, int month, double length);
    void DeleteWeight(string id, int month);
    void EditWeight(string id, int month, double length);
    List<double> GetWeightReferenceData(string gender, int percentile);
    List<double> GetWeight(string id);

}