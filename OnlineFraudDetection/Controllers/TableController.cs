using Microsoft.AspNetCore.Mvc;
using OnlineFraudDetection.ApiHelper;

namespace OnlineFraudDetection.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TableController : Controller
{
    private readonly IApiHelper _apiHelper;

    public TableController(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }
    
    [HttpDelete]
    public async Task DeleteTableRows()
    {
        await _apiHelper.DeleteTableRows();
    }

}