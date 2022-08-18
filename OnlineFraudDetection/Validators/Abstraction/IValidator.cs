using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.Validators.Abstraction;

public interface IValidator
{
    int GetTransactionFraudPercentage(Transaction transaction, Profile profile);
}