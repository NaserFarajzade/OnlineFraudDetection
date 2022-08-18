using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Validators.Abstraction;

namespace OnlineFraudDetection.Validators.Implementation;

public class Validator:IValidator
{
    private readonly Settings _settings;
    private List<Tuple<int, IFraudRuleValidator>> _tuples;
    public Validator(Settings settings)
    {
        _settings = settings;
        _tuples = new List<Tuple<int, IFraudRuleValidator>>
        {
            new(_settings.FraudulentFactorCoefficient.Time,new TimeRuleValidator(_settings)),
            new(_settings.FraudulentFactorCoefficient.BankType,new BankTypeRuleValidator(_settings)),
            new(_settings.FraudulentFactorCoefficient.CardsCount,new CardsCountRuleValidator(_settings)),
            new(_settings.FraudulentFactorCoefficient.ExceedingTheAverage,new ExceedingTheAverageRuleValidator(_settings))
        };
    }

    public int GetTransactionFraudPercentage(Transaction transaction, Profile profile)
    {
        var sumOfFraudCoeffs = _settings.FraudulentFactorCoefficient.Time +
                                _settings.FraudulentFactorCoefficient.BankType +
                                _settings.FraudulentFactorCoefficient.CardsCount +
                                _settings.FraudulentFactorCoefficient.ExceedingTheAverage;
        
        var transactionFraudSum = 0;
        
        foreach (var (coeff, validator) in _tuples)
        {
            transactionFraudSum += coeff * ManageValidator(validator,transaction,profile);
        }

        return transactionFraudSum / sumOfFraudCoeffs;
    }

    private int ManageValidator(IFraudRuleValidator validator, Transaction transaction, Profile profile)
    {
        return validator switch
        {
            TimeRuleValidator timeRuleValidator => timeRuleValidator.GetPercentage(transaction.TransactionDateTime.Hour, null),
            BankTypeRuleValidator bankTypeRuleValidator => bankTypeRuleValidator.GetPercentage(transaction.OriginCard.Substring(0, 6), null),
            CardsCountRuleValidator cardsCountRuleValidator => cardsCountRuleValidator.GetPercentage(null, profile.AccountHolderCardsCount),
            ExceedingTheAverageRuleValidator exceedingTheAverageRuleValidator => exceedingTheAverageRuleValidator.GetPercentage(transaction.TransactionAmount, profile.TransactionsAmountAverage),
            _ => throw new Exception("validator not found")
        };
    }
}