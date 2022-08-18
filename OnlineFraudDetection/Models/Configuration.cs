namespace OnlineFraudDetection.Models;

public class Settings
{
    public string TransactionFolder { get; set; }
    public string AccountHolderFolder { get; set; }
    public char SeparatorCharacter { get; set; } = ',';
    public int BulkSize { get; set; } = 1;
    public int AccountHolderMaximumCardsCount { get; set; }
    public int ToleranceCoefficient { get; set; }
    public List<string> SuspiciousBanks { get; set; }
    public List<int> FraudulentTransactionTimes { get; set; }
    public TransactionFieldsIndexSettings TransactionFieldsIndex { get; set; }
    public AccountHoldersFieldsIndexSettings AccountHoldersFieldsIndex { get; set; }
    public FraudulentFactorCoefficientSettings FraudulentFactorCoefficient { get; set; }
    public FraudulentFactorNormalizationSettings FraudulentFactorNormalizationValue { get; set; }
    public RedisCache redisCache { get; set; }
}

public class RedisCache
{
    public bool EnableCaching { get; set; }
    public string IP { get; set; }
    public int Port { get; set; }
}

public class FraudulentFactorNormalizationSettings
{
    public double CardsCount {get; set;} 
    public double ExceedingTheAverage {get; set;}    
}

public class FraudulentFactorCoefficientSettings
{
    public int Time {get; set;}
    public int CardsCount {get; set;}
    public int BankType {get; set;}
    public int ExceedingTheAverage {get; set;}
}

public class TransactionFieldsIndexSettings
{
    public int OriginCard {get; set;}
    public int DestinationCard {get; set;}
    public int TransactionDateTime {get; set;}
    public int TransactionAmount {get; set;}
    public int TransactionReference {get; set;}
    
}

public class AccountHoldersFieldsIndexSettings
{
    public int Name {get; set;}
    public int NationalCode {get; set;}
    public int CardNumber {get; set;}
}

public class FraudulentFactorCoefficient
{
    public int Time { get; set; }
    public int CardsCount { get; set; }
    public int BankType { get; set; }
    public int ExceedingTheAverage { get; set; }
}