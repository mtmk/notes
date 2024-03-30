using StackExchange.Redis;

// RedisExample("192.168.0.21:6379");
RedisExample("127.0.0.1:6379");
    
void RedisExample(string configuration)
{
    // Connect to Redis server
    using var redis = ConnectionMultiplexer.Connect(configuration);

    // Get a database
    var db = redis.GetDatabase();

    // Set a key
    db.StringSet("key", "value");

    // Get a key
    var value = db.StringGet("key");

    Console.WriteLine(value); // value
}
