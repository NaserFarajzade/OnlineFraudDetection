using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineFraudDetection.ApiHelper;

namespace OnlineFraudDetection.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class FraudDetectionController : Controller
{
    private readonly IApiHelper _apiHelper;

    public FraudDetectionController(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    
    [HttpPost]
    public async Task<string> GetFraudPercentage(Transaction transaction)
    {
        return await _apiHelper.GetFraudPercentage(transaction);
    }
}