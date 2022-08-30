namespace OnlineFraudDetection.Models;

public class FraudResult
{
    public string AccountHolderName { get; set; }
    public string CardNumber { get; set; }
    public bool IsFraud { get; set; }

    public float TotalElapsedTime { get; set; }
    public float TimeRuleValidationDuration { get; set; }
    public float BankTypeRuleValidationDuration { get; set; }
    public float CardsCountRuleValidationDuration { get; set; }
    public float ExceedingTheAverageRuleValidationDuration { get; set; }
    public float DataBaseRequestDuration { get; set; }

    public int Percentage { get; set; }
    public int TimeRuleValidationPercentage { get; set; }
    public int BankTypeRuleValidationPercentage { get; set; }
    public int CardsCountRuleValidationPercentage { get; set; }
    public int ExceedingTheAverageRuleValidationPercentage { get; set; }
    public bool IsRedisCacheEnabled { get; set; }

    public bool TransactionLabel { get; set; }
    public bool ResultLabel { get; set; }
}