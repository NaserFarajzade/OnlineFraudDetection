using OnlineFraudDetection.Models;
using StackExchange.Redis;

namespace OnlineFraudDetection.RedisCache;

public class RedisConnectionManager
{
    public RedisConnectionManager(Settings settings)
    {
        lock (locker)
        {
            if (lazyConnection == null)
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(
                    () => ConnectionMultiplexer.Connect($"{settings.redisCache.IP}:{settings.redisCache.Port}",
                        options => options.AllowAdmin = true)
                    );
            }
        }
    }
    private static Lazy<ConnectionMultiplexer> lazyConnection;
    private static readonly object locker = new object();
    public ConnectionMultiplexer Connection { get { return lazyConnection.Value; } }
}