using EFDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EFDataAccessLibrary.DataAccess;

public class DataBaseContext: DbContext
{
    public DataBaseContext(DbContextOptions options): base(options){}
    
    public DbSet<AccountHolder> AccountHolders { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Profile> Profiles { get; set; }
}