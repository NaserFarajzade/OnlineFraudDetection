using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation.Utility;

namespace OnlineFraudDetection.Validators.Implementation;

public class CardsCountRuleValidator:IFraudRuleValidator,IResultPropertySetter
{
    private readonly Settings _settings;

    public CardsCountRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(Transaction transaction, Profile profile)
    {
        var accountHolderCardsCount = profile.AccountHolderCardsCount;
        var maxCardsCount = _settings.AccountHolderMaximumCardsCount;
        if (accountHolderCardsCount < maxCardsCount) return 0;
        return SigmoidFunction.Calculate(accountHolderCardsCount, _settings.FraudulentFactorNormalizationValue.CardsCount, maxCardsCount);
    }

    public void SetRuleDuration(FraudResult result, float duration)
    {
        result.CardsCountRuleValidationDuration = duration;
    }

    public void SetRulePercentage(FraudResult result, int rulePercentage)
    {
        result.CardsCountRuleValidationPercentage = rulePercentage;
    }
}