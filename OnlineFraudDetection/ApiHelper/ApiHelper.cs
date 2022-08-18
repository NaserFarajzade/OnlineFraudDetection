using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Repositories.Abstraction;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation;

namespace OnlineFraudDetection.ApiHelper;

public class ApiHelper:IApiHelper
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountHolderRepository _accountHolderRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly Settings _settings;
    private readonly IValidator _validator;

    public ApiHelper(
        ITransactionRepository transactionRepository, 
        IAccountHolderRepository accountHolderRepository, 
        IProfileRepository profileRepository, 
        Settings settings,
        IValidator validator)
    {
        _transactionRepository = transactionRepository;
        _accountHolderRepository = accountHolderRepository;
        _profileRepository = profileRepository;
        _settings = settings;
        _validator = validator;
    }

    public async Task<string> GetFraudPercentage(Transaction transaction)
    {
        var profile = await _profileRepository.Get(transaction.OriginCard);
        if (profile is null) return "profile not found";
        return _validator.GetTransactionFraudPercentage(transaction,profile).ToString();
    }

    public async Task CreateProfiles()
    {
        await DeleteTableRows();
        await AddInformationFromDirectoryToDataBase();
        
        var cardsNumber = await _accountHolderRepository.GetAllCardsNumber();
        foreach (var cardNumber in cardsNumber)
        {
            var amountAvg = await _transactionRepository.GetTransactionAmountAverage(cardNumber);
            var accountHolderCardsCount = await _accountHolderRepository.GetAccountHolderCardsCount(cardNumber);
            //TODO behbood performance

            await _profileRepository.AddAsync(new Profile()
            {
                CardNumber = cardNumber,
                TransactionsAmountAverage = amountAvg,
                AccountHolderCardsCount = accountHolderCardsCount
            });
        }
    }
    public async Task CreateProfileWithNewDataSets(List<string> accountHolderDataSetFileNames, List<string> transactionDataSetFileNames)
    {
        foreach (var accountHolderDataSetFileName in accountHolderDataSetFileNames)
        {
            await AddAccountHolderFromFileToDataBase(accountHolderDataSetFileName);
        }

        foreach (var transactionDataSetFileName in transactionDataSetFileNames)
        {
            await AddTransactionFromFileToDataBase(transactionDataSetFileName);
        }
    }
    public async Task DeleteTableRows()
    {
        await _profileRepository.DeleteAll();
        await _transactionRepository.DeleteAll();
        await _accountHolderRepository.DeleteAll();
    }
    private async Task AddInformationFromDirectoryToDataBase()
    {
        foreach (var transactionFile in Directory.GetFiles(_settings.TransactionFolder))
        {
            await AddTransactionFromFileToDataBase(Path.GetFileName(transactionFile));
        }
        foreach (var accountHolderFile in Directory.GetFiles(_settings.AccountHolderFolder))
        {
            await AddAccountHolderFromFileToDataBase(Path.GetFileName(accountHolderFile));
        }
    }
    private async Task AddAccountHolderFromFileToDataBase(string accountHolderFileName)
    {
        var accountHolderFileAddress = Path.Combine(_settings.AccountHolderFolder, accountHolderFileName);
        var bulk = new List<AccountHolder>();
        foreach (var line in File.ReadLines(accountHolderFileAddress))
        {
            var fields = line.Split(_settings.SeparatorCharacter);
            var accountHolder = new AccountHolder()
            {
                Name = fields[_settings.AccountHoldersFieldsIndex.Name].Trim(),
                CardNumber = fields[_settings.AccountHoldersFieldsIndex.CardNumber].Trim(),
                NationalCode = fields[_settings.AccountHoldersFieldsIndex.NationalCode].Trim()
            };
            bulk.Add(accountHolder);
        }

        if (bulk.Any())
        {
            await _accountHolderRepository.AddRangeAsync(bulk,_settings.BulkSize);
        }
    }
    private async Task AddTransactionFromFileToDataBase(string transactionFileName)
    {
        var transactionFileAddress = Path.Combine(_settings.TransactionFolder, transactionFileName);
        var bulk = new List<Transaction>();
        foreach (var line in File.ReadLines(transactionFileAddress))
        {
            var fields = line.Split(_settings.SeparatorCharacter);
            var transaction = new Transaction()
            {
                OriginCard = fields[_settings.TransactionFieldsIndex.OriginCard].Trim(),
                DestinationCard = fields[_settings.TransactionFieldsIndex.DestinationCard].Trim(),
                TransactionAmount = long.Parse(fields[_settings.TransactionFieldsIndex.TransactionAmount]),
                TransactionDateTime = Convert.ToDateTime(fields[_settings.TransactionFieldsIndex.TransactionDateTime]),
                TransactionReference = fields[_settings.TransactionFieldsIndex.TransactionReference].Trim()
            };
            
            bulk.Add(transaction);
        }

        if (bulk.Any())
        {
            await _transactionRepository.AddRangeAsync(bulk,_settings.BulkSize);
        }
        
    }
}