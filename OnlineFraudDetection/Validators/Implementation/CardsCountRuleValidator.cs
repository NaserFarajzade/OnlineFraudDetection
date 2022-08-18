using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation.Utility;

namespace OnlineFraudDetection.Validators.Implementation;

public class CardsCountRuleValidator:IFraudRuleValidator
{
    private readonly Settings _settings;

    public CardsCountRuleValidator(Settings settings)
    {
        _settings = settings;
    }

    public int GetPercentage(object fromTransaction, object fromProfile)
    {
        var accountHolderCardsCount = (int)fromProfile;
        var maxCardsCount = _settings.AccountHolderMaximumCardsCount;
        if (accountHolderCardsCount < maxCardsCount) return 0;
        return SigmoidFunction.Calculate(accountHolderCardsCount, _settings.FraudulentFactorNormalizationValue.CardsCount, maxCardsCount);
    }

}