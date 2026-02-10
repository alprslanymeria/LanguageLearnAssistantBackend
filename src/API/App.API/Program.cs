using App.API.ExceptionHandlers;
using App.API.Extensions;
using App.API.Middlewares;
using App.API.ModelBinding;
using App.Application;
using App.Application.Common.Behaviors;
using App.Caching;
using App.Integration.ExternalApi;
using App.Integration.Mapping;
using App.Integration.Translation;
using App.Observability;
using App.Storage;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// ADD APPSETTINGS JSON FILES
builder.Configuration
    .AddJsonFile("appsettings.observability.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.caching.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Translation.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.storage.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.database.json", optional: false, reloadOnChange: true);

// OPEN TELEMETRY
builder.AddOpenTelemetryLogExt();

// SERVICES
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new FileUploadModelBinderProvider());
});
builder.Services.AddOpenApi();
builder.Services
    .AddPersistenceServicesExt(builder.Configuration)
    .AddOpenTelemetryServicesExt(builder.Configuration)
    .AddCachingServicesExt(builder.Configuration)
    .AddStorageServicesExt(builder.Configuration)
    .AddMappingServicesExt()
    .AddCustomTokenAuthExt(builder.Configuration)
    .AddApplicationServicesExt()
    .AddExternalApiServicesExt(builder.Configuration)
    .AddTranslationServicesExt(builder.Configuration)
    .AddApiVersioningExt()
    .AddRateLimitingExt();

// MEDIATR WITH PIPELINE BEHAVIORS
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly);
    cfg.AddOpenBehavior(typeof(ExceptionHandlerBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
});

// EXCEPTION HANDLERS
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// FLUENT VALIDATION
builder.Services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// MIDDLEWARES
app.UseExceptionHandler(x => { });
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
