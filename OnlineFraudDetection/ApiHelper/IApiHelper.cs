using EFDataAccessLibrary.Models;

namespace OnlineFraudDetection.ApiHelper;

public interface IApiHelper
{
    Task<string> GetFraudPercentage(Transaction transaction);
    Task CreateProfiles();
    Task CreateProfileWithNewDataSets(List<string> accountHolderDataSetFileNames, List<string> transactionDataSetFileNames);
    Task DeleteTableRows();
    Task EnableRedis(bool beEnable);
}