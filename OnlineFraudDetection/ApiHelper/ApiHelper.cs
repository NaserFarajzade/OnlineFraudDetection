using System.Diagnostics;
using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.RedisCache;
using OnlineFraudDetection.Repositories.Abstraction;
using OnlineFraudDetection.Validators.Abstraction;
using Serilog;

namespace OnlineFraudDetection.ApiHelper;

public class ApiHelper:IApiHelper
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountHolderRepository _accountHolderRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IRedisRepository _redisRepository;
    private readonly Settings _settings;
    private readonly IValidator _validator;
    private readonly Random _random;

    public ApiHelper(
        ITransactionRepository transactionRepository, 
        IAccountHolderRepository accountHolderRepository, 
        IProfileRepository profileRepository, 
        IRedisRepository redisRepository,
        Settings settings,
        IValidator validator)
    {
        _transactionRepository = transactionRepository;
        _accountHolderRepository = accountHolderRepository;
        _profileRepository = profileRepository;
        _redisRepository = redisRepository;
        _settings = settings;
        _validator = validator;
        _random = new Random();
    }

    public async Task<string> GetFraudPercentage(Transaction transaction)
    {
        Profile? profile;
        var stopWatch = Stopwatch.StartNew();
        if (_settings.redisCache.EnableCaching)
        {
            if (!_redisRepository.TryGetProfile(transaction.OriginCard,out profile))
            {
                profile = await _profileRepository.Get(transaction.OriginCard);
                if (profile is null) return "profile not found";
                _redisRepository.AddProfile(profile);
            }
        }
        else
        {
            profile = await _profileRepository.Get(transaction.OriginCard);
        }
        var dbRequestDuration = (float)stopWatch.Elapsed.Ticks / 10; //micro seconds
        
        if (profile is null) return "profile not found";
        var result = _validator.GetTransactionFraudResult(transaction, profile);
        
        var totalElapsedTime = (float)stopWatch.Elapsed.Ticks / 10; //micro seconds 
        result.DataBaseRequestDuration = dbRequestDuration;
        result.TotalElapsedTime = totalElapsedTime;
        result.IsRedisCacheEnabled = _settings.redisCache.EnableCaching;
        result.TransactionLabel = transaction.IsFraudulentLabel;
        result.ResultLabel = result.Percentage >= _settings.FraudPercentageThreshold;

        var timeLog = new SingleValueLog()
        {
            Id = transaction.Id,
            Type = "TimeLog",
            Duration = result.TimeRuleValidationDuration,
            Percentage = result.TimeRuleValidationPercentage,
            IsRedisCacheEnabled = _settings.redisCache.EnableCaching
        };
        var bankLog = new SingleValueLog()
        {
            Id = transaction.Id,
            Type = "BankLog",
            Duration = result.BankTypeRuleValidationDuration,
            Percentage = result.BankTypeRuleValidationPercentage,
            IsRedisCacheEnabled = _settings.redisCache.EnableCaching
        };
        var cardsCountLog = new SingleValueLog()
        {
            Id = transaction.Id,
            Type = "CardsCountLog",
            Duration = result.CardsCountRuleValidationDuration,
            Percentage = result.CardsCountRuleValidationPercentage,
            IsRedisCacheEnabled = _settings.redisCache.EnableCaching
        };
        var exceedingTheAverageLog = new SingleValueLog()
        {
            Id = transaction.Id,
            Type = "ExceedingTheAverageLog",
            Duration = result.ExceedingTheAverageRuleValidationDuration,
            Percentage = result.ExceedingTheAverageRuleValidationPercentage,
            IsRedisCacheEnabled = _settings.redisCache.EnableCaching
        };
        var dataBaseRequestLog = new SingleValueLog()
        {
            Id = transaction.Id,
            Type = "DataBaseRequestLog",
            Duration = result.DataBaseRequestDuration,
            IsRedisCacheEnabled = _settings.redisCache.EnableCaching
        };
        var totalResultLog = new SingleValueLog()
        {
            Id = transaction.Id,
            Type = "TotalResultLog",
            Duration = result.TotalElapsedTime,
            Percentage = result.Percentage,
            IsRedisCacheEnabled = _settings.redisCache.EnableCaching
        };
        
        
        Log.Information("{@SingleValueLog}",timeLog);
        Log.Information("{@SingleValueLog}",bankLog);
        Log.Information("{@SingleValueLog}",cardsCountLog);
        Log.Information("{@SingleValueLog}",exceedingTheAverageLog);
        Log.Information("{@SingleValueLog}",dataBaseRequestLog);
        Log.Information("{@SingleValueLog}",totalResultLog);
        Log.Information("{@FraudResult}",result);
        
        return result.Percentage.ToString();
    }

    public async Task CreateProfiles()
    {
        await DeleteTableRows();
        await AddInformationFromDirectoryToDataBase();
        await BuildProfiles();
    }
    public async Task CreateProfileWithNewDataSets(List<string>? accountHolderDataSetFileNames, List<string>? transactionDataSetFileNames)
    {
        if (accountHolderDataSetFileNames != null && accountHolderDataSetFileNames.Count != 0)
        {
            foreach (var accountHolderDataSetFileName in accountHolderDataSetFileNames)
            {
                await AddAccountHolderFromFileToDataBase(accountHolderDataSetFileName);
            }
        }

        if (transactionDataSetFileNames != null && transactionDataSetFileNames.Count != 0)
        {
            foreach (var transactionDataSetFileName in transactionDataSetFileNames)
            {
                await AddTransactionFromFileToDataBase(transactionDataSetFileName);
            }
        }

        await _profileRepository.DeleteAll();
        await BuildProfiles();
    }
    public async Task DeleteTableRows()
    {
        await _profileRepository.DeleteAll();
        await _transactionRepository.DeleteAll();
        await _accountHolderRepository.DeleteAll();
        if (_settings.redisCache.EnableCaching)
        {
            await _redisRepository.DeleteAll();
        }
    }

    public Task EnableRedis(bool beEnable)
    {
        _settings.redisCache.EnableCaching = beEnable;
        return Task.CompletedTask;
    }

    public Task<bool> IsRedisEnable()
    {
        return Task.FromResult(_settings.redisCache.EnableCaching);
    }

    private async Task BuildProfiles()
    {
        var accountHolderNameAndCardNumbers = await _accountHolderRepository.GetAllAccountHolderNameAndCardNumbers();
        foreach (var tuple in accountHolderNameAndCardNumbers)
        {
            var amountAvg = await _transactionRepository.GetTransactionAmountAverage(tuple.Item1);
            var accountHolderCardsCount = await _accountHolderRepository.GetAccountHolderCardsCount(tuple.Item1);

            var profile = new Profile()
            {
                CardNumber = tuple.Item1,
                Name = tuple.Item2,
                TransactionsAmountAverage = amountAvg,
                AccountHolderCardsCount = accountHolderCardsCount
            };
            if (_settings.redisCache.EnableCaching)
            {
                _redisRepository.AddProfile(profile);
            }

            await _profileRepository.AddAsync(profile);
        }
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