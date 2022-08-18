using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;

namespace OnlineFraudDetection.Validators.Implementation;

public class BankTypeRuleValidator:IFraudRuleValidator
{
    private readonly Settings _settings;

    public BankTypeRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(object fromTransaction, object fromProfile)
    {
        var bankType = (string)fromTransaction;
        return _settings.SuspiciousBanks.Contains(bankType) ? 100 : 0;
    }
}