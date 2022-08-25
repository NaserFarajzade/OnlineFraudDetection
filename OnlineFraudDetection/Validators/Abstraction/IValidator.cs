using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;

namespace OnlineFraudDetection.Validators.Abstraction;

public interface IValidator
{
    FraudResult GetTransactionFraudResult(Transaction transaction, Profile profile);
}