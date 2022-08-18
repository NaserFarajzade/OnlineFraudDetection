using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using OnlineFraudDetection.Repositories.Abstraction;

namespace OnlineFraudDetection.Repositories.Implementation;

public class ProfileRepository:IProfileRepository
{
    private readonly DataBaseContext _context;

    public ProfileRepository(DataBaseContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Profile profile)
    {
        await _context.Profiles.AddAsync(profile);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Profile> profiles)
    {
        await _context.Profiles.AddRangeAsync(profiles);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAll()
    {
        _context.Profiles.RemoveRange(_context.Profiles);
        await _context.SaveChangesAsync();
    }

    public Task<Profile> Get(string cardNumber)
    {
        var profile = _context.Profiles.SingleOrDefault(profile => profile.CardNumber == cardNumber);
        return Task.FromResult(profile)!;
    }
}