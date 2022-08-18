using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;

namespace OnlineFraudDetection.Validators.Implementation;

public class TimeRuleValidator:IFraudRuleValidator
{
    private readonly Settings _settings;

    public TimeRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(object fromTransaction, object fromProfile)
    {
        var hour = (int)fromTransaction;
        return _settings.FraudulentTransactionTimes.Contains(hour) ? 100 : 0;
    }
}