using Microsoft.AspNetCore.Mvc;
using OnlineFraudDetection.ApiHelper;
using OnlineFraudDetection.Models;

namespace OnlineFraudDetection.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IApiHelper _apiHelper;

    public ProfileController(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    [HttpGet]
    public async Task CreateProfile()
    {
        await _apiHelper.CreateProfiles();
    }
    
    [HttpPost]
    public async Task CreateProfileWithNewDataSets(DataSetsAddress addresses)
    {
        await _apiHelper.CreateProfileWithNewDataSets(addresses.accountHolderDataSetFileNames, addresses.transactionDataSetFileNames);
    }
    
}