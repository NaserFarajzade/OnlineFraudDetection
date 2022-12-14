using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.Repositories.Abstraction;

public interface IAccountHolderRepository
{
    Task AddAsync(AccountHolder accountHolder);
    Task AddRangeAsync(IList<AccountHolder> accountHolders, int batchSize);
    Task DeleteAll();
    Task<IEnumerable<Tuple<string,string>>> GetAllAccountHolderNameAndCardNumbers();
    Task<int> GetAccountHolderCardsCount(string cardNumber);
    Task<AccountHolder?> Get(string cardNumber);
}