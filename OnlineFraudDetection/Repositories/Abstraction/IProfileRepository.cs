using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.Repositories.Abstraction;

public interface IProfileRepository
{
    Task AddAsync(Profile profile);
    Task AddRangeAsync(IEnumerable<Profile> profiles);
    Task DeleteAll();
    Task<Profile> Get(string cardNumber);

}