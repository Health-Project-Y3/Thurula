using thurula.Models;

namespace thurula.Services;

public class BmiService : IBmiService
{
    private readonly IUserService _userService;

    public BmiService(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Calculates the BMI
    /// </summary>
    /// <param name="height">Height in cm</param>
    /// <param name="weight">Weight in kg</param>
    /// <returns>BMI eg. 18.5</returns>
    public double CalculateBmi(double height, double weight)
    {
        var h2 = (height / 100) * (height / 100);
        var bmi = weight / h2;
        return bmi;
    }

    /// <summary> Checks whether the BMI falls within the correct range for pregnant mothers</summary>
    /// <param name="user"></param>
    /// <returns>high, average, low</returns>
    public string CheckRange(User user)
    {
        var preWeight = user.PreWeight;
        var nowWeight = user.Weight;
        var weightGain = nowWeight - preWeight;
        var preBmi = CalculateBmi(user.Height, preWeight);
        double r1, r2;
        switch (preBmi)
        {
            case < 18.5:
                r1 = 13;
                r2 = 18;
                break;
            case >= 18.5 and < 26:
                r1 = 11;
                r2 = 16;
                break;
            case >= 26 and < 30:
                r1 = 7;
                r2 = 11;
                break;
            default:
                r1 = 0;
                r2 = 7;
                break;
        }

        if (weightGain >= r2)
            return "high";
        return weightGain <= r1 ? "low" : "average";
    }

}