using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;

namespace OnlineFraudDetection.Validators.Implementation;

public class TimeRuleValidator:IFraudRuleValidator,IResultPropertySetter
{
    private readonly Settings _settings;

    public TimeRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(Transaction transaction, Profile profile)
    {
        var hour = transaction.TransactionDateTime.Hour;
        return _settings.FraudulentTransactionTimes.Contains(hour) ? 100 : 0;
    }

    public void SetRuleDuration(FraudResult result, float duration)
    {
        result.TimeRuleValidationDuration = duration;
    }

    public void SetRulePercentage(FraudResult result, int rulePercentage)
    {
        result.TimeRuleValidationPercentage = rulePercentage;
    }
}