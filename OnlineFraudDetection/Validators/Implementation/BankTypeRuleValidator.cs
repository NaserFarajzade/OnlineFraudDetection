using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;

namespace OnlineFraudDetection.Validators.Implementation;

public class BankTypeRuleValidator:IFraudRuleValidator,IResultPropertySetter
{
    private readonly Settings _settings;

    public BankTypeRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(Transaction transaction, Profile profile)
    {
        var bankType = transaction.OriginCard.Substring(0, 6);
        return _settings.SuspiciousBanks.Contains(bankType) ? 100 : 0;
    }

    public void SetRuleDuration(FraudResult result, float duration)
    {
        result.BankTypeRuleValidationDuration = duration;
    }

    public void SetRulePercentage(FraudResult result, int rulePercentage)
    {
        result.BankTypeRuleValidationPercentage = rulePercentage;
    }
}