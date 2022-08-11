namespace OnlineFraudDetection.Models;

public class Settings
{
    public string TransactionFolder { get; set; }
    public string AccountHolderFolder { get; set; }
    public int AccountHolderMaximumCardsCount { get; set; }
    public int ToleranceCoefficient { get; set; }
    public List<string> SuspiciousBanks { get; set; }
    public List<int> FraudulentTransactionTimes { get; set; }
    public TransactionFieldsIndexSettings TransactionFieldsIndex { get; set; }
    public AccountHoldersFieldsIndexSettings AccountHoldersFieldsIndex { get; set; }
}

public class TransactionFieldsIndexSettings
{
    public int OriginCard {get; set;}
    public int DestinationCard {get; set;}
    public int TransactionDate {get; set;}
    public int TransactionTime {get; set;}
    public int TransactionAmount {get; set;}
    public int TransactionReference {get; set;}
    public int TransactionId {get; set;}
    
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