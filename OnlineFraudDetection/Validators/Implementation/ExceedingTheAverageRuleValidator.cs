using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation.Utility;

namespace OnlineFraudDetection.Validators.Implementation;

public class ExceedingTheAverageRuleValidator:IFraudRuleValidator,IResultPropertySetter
{
    private readonly Settings _settings;

    public ExceedingTheAverageRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(Transaction transaction, Profile profile)
    {
        var transactionAmount = Convert.ToDouble(transaction.TransactionAmount);
        var averageAmount = profile.TransactionsAmountAverage;
        if (averageAmount == 0) return 0;
        if (transactionAmount < averageAmount) return 0;
        return SigmoidFunction.Calculate(transactionAmount, _settings.FraudulentFactorNormalizationValue.ExceedingTheAverage, averageAmount);
    }

    public void SetRuleDuration(FraudResult result, float duration)
    {
        result.ExceedingTheAverageRuleValidationDuration = duration;
    }

    public void SetRulePercentage(FraudResult result, int rulePercentage)
    {
        result.ExceedingTheAverageRuleValidationPercentage = rulePercentage;
    }
}