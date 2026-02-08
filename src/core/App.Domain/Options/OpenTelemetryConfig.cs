namespace App.Domain.Options;

public class OpenTelemetryConfig
{
    public const string Key = "OpenTelemetryConfig";

    /// <summary>
    /// ENABLE OR DISABLE OPENTELEMETRY
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// THE NAME OF THE APPLICATION/SERVICE THAT COLLECTS TELEMETRY DATA
    /// </summary>
    public string ServiceName { get; set; } = null!;

    /// <summary>
    /// VERSION OF THE SERVICE
    /// </summary>
    public string ServiceVersion { get; set; } = null!;

    /// <summary>
    /// TRACE SOURCE IN APPLICATION/SERVICE
    /// </summary>
    public string ActivitySourceName { get; set; } = null!;

    /// <summary>
    /// LOG EXPORTERS TO USE
    /// </summary>
    public string[] LogExporters { get; set; } = ["console", "elasticsearch"];

    /// <summary>
    /// TRACE EXPORTERS TO USE
    /// </summary>
    public string[] TraceExporters { get; set; } = ["jaeger"];

    /// <summary>
    /// METRICS EXPORTERS TO USE
    /// </summary>
    public string[] MetricsExporters { get; set; } = ["prometheus"];
}
