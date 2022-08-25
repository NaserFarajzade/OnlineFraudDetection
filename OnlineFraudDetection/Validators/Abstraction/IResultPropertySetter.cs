using OnlineFraudDetection.Models;

namespace OnlineFraudDetection.Validators.Abstraction;

public interface IResultPropertySetter
{
    public void SetRuleDuration(FraudResult result, float duration);
    public void SetRulePercentage(FraudResult result, int rulePercentage);
}