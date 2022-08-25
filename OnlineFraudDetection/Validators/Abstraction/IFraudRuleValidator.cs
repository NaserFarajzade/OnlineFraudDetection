using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.Validators.Abstraction;

public interface IFraudRuleValidator
{
    int GetPercentage(Transaction transaction, Profile profile);
}