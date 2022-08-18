using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation.Utility;

namespace OnlineFraudDetection.Validators.Implementation;

public class ExceedingTheAverageRuleValidator:IFraudRuleValidator
{
    private readonly Settings _settings;

    public ExceedingTheAverageRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(object fromTransaction, object fromProfile)
    {
        var transactionAmount = Convert.ToDouble(fromTransaction);
        var averageAmount = (long)fromProfile;
        if (averageAmount == 0) return 0;
        if (transactionAmount < averageAmount) return 0;
        return SigmoidFunction.Calculate(transactionAmount, _settings.FraudulentFactorNormalizationValue.ExceedingTheAverage, averageAmount);
    }
}