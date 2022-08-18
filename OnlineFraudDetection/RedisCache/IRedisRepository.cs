using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.RedisCache;

public interface IRedisRepository
{
    public bool AddProfile(Profile profile);
    public Profile GetProfile(string cardNumber);
    public bool TryGetProfile(string cardNumber, out Profile? profile);
    public bool TryGetAccountHolderCardsCount(string cardNumber, out Profile profile);
    Task DeleteAll();
}