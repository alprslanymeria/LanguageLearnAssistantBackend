namespace App.Domain.Options.Database;

/// <summary>
/// CONFIGURATION OPTIONS FOR DATABASE CONNECTION.
/// </summary>
public class DatabaseConfig
{
    public const string Key = "DatabaseConfig";

    /// <summary>
    /// DATABASE ENVIRONMENT
    /// </summary>
    public DatabaseType Environment { get; set; } = DatabaseType.Local;
}
