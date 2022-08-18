namespace OnlineFraudDetection.Validators.Abstraction;

public interface IFraudRuleValidator
{
    int GetPercentage(object fromTransaction, object fromProfile);
}