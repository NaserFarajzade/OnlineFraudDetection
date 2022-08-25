using System.Diagnostics;
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

    public FraudResult GetTransactionFraudResult(Transaction transaction, Profile profile)
    {
        var transactionFraudSum = 0;
        var result = new FraudResult();
        var stopwatch = new Stopwatch();
        foreach (var (coeff, validator) in _tuples)
        {
            stopwatch.Start();
            var rulePercentage = coeff * validator.GetPercentage(transaction, profile); 
            transactionFraudSum += rulePercentage;
            stopwatch.Stop();
            var duration = (float)stopwatch.Elapsed.Ticks / 10; //micro seconds
            ((IResultPropertySetter)validator).SetRuleDuration(result,duration);
            if (_settings.ShowRulesPercentageInLog)
            {
                ((IResultPropertySetter)validator).SetRulePercentage(result,rulePercentage / 100);
            }
            stopwatch.Reset();
        }

        result.AccountHolderName = profile.Name;
        result.CardNumber = transaction.OriginCard;
        result.Percentage = transactionFraudSum / 100;
        result.IsFraud = result.Percentage >= _settings.FraudPercentageThreshold;
        return result;
    }
}