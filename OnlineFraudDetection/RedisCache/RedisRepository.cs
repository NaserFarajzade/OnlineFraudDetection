using System.Net;
using System.Text.Json;
using EFDataAccessLibrary.Models;
using StackExchange.Redis;

namespace OnlineFraudDetection.RedisCache;

public class RedisRepository:IRedisRepository
{
    private IDatabase _db;
    private readonly EndPoint[] _endPoints;
    private readonly ConnectionMultiplexer _connection;
    private string _profileLabel = "profile";

    public RedisRepository(RedisConnectionManager manager)
    {
        _connection = manager.Connection;
        _endPoints = manager.Connection.GetEndPoints(true);
        _db = manager.Connection.GetDatabase();
    }
    
    public bool AddProfile(Profile profile)
    {
        var serialized = SerializeProfile(profile);
        var labeledKey = AddLabelToKey(_profileLabel, profile.CardNumber); 
        return _db.StringSet(new RedisKey(labeledKey), new RedisValue(serialized));
    }

    public Profile GetProfile(string cardNumber)
    {
        var labeledKey = AddLabelToKey(_profileLabel, cardNumber);
        string? json = _db.StringGet(new RedisKey(labeledKey));
        return DeserializeProfile(json);
    }

    public bool TryGetProfile(string cardNumber, out Profile? profile)
    {
        var labeledKey = AddLabelToKey(_profileLabel, cardNumber);
        var value = _db.StringGet(new RedisKey(labeledKey));
        if (value.HasValue)
        {
            profile = DeserializeProfile(value.ToString());
            return true;
        }

        profile = null;
        return false;
    }

    public bool TryGetAccountHolderCardsCount(string cardNumber, out Profile profile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAll()
    {
        return Parallel.ForEachAsync(_endPoints, ((endPoint, token) =>
        {
            var server = _connection.GetServer(endPoint);
            server.FlushAllDatabases();
            return default;
        }));
    }

    private string SerializeProfile(Profile profile)
    {
        return JsonSerializer.Serialize(profile);
    }
    
    private Profile? DeserializeProfile(string json)
    {
        return JsonSerializer.Deserialize<Profile>(json);
    }

    private string AddLabelToKey(string label, string key) => $"{label}_{key}";
}