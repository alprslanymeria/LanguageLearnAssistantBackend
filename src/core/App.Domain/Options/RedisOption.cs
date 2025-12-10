namespace App.Domain.Options;

public class RedisOption
{
    public const string Key = "Redis";
    public string ConnectionString { get; set; } = default!;
    public int Database { get; set; }
}
