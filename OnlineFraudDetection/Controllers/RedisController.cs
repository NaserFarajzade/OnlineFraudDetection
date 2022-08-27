using Microsoft.AspNetCore.Mvc;
using OnlineFraudDetection.ApiHelper;

namespace OnlineFraudDetection.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RedisController : ControllerBase
{
    private readonly IApiHelper _apiHelper;

    public RedisController(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    [HttpGet]
    public async Task EnableRedis(bool beEnable)
    {
        await _apiHelper.EnableRedis(beEnable);
    }
    
    [HttpGet]
    public async Task<bool> IsRedisEnable()
    {
        return await _apiHelper.IsRedisEnable();
    }
}