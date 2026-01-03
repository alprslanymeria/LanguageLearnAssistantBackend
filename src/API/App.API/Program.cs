using App.API.ExceptionHandlers;
using App.API.Extensions;
using App.API.Filters;
using App.API.Middlewares;
using App.Application;
using App.Application.Common.Behaviors;
using App.Caching;
using App.Integration.ExternalApi;
using App.Integration.Mapping;
using App.Integration.Translation;
using App.Observability;
using App.Storage;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// OPEN TELEMETRY
builder.AddOpenTelemetryLogExt();

// SERVICES
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddPersistenceServicesExt(builder.Configuration)
    .AddOpenTelemetryServicesExt(builder.Configuration)
    .AddCachingServicesExt(builder.Configuration)
    .AddStorageServicesExt()
    .AddMappingServicesExt()
    .AddCustomTokenAuthExt(builder.Configuration)
    .AddOptionsPatternExt(builder.Configuration)
    .AddApplicationServicesExt()
    .AddExternalApiServicesExt(builder.Configuration)
    .AddTranslationServicesExt(builder.Configuration)
    .AddApiVersioningExt()
    .AddRateLimitingExt();

// MEDIATR WITH PIPELINE BEHAVIORS
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// EXCEPTION HANDLERS
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// FLUENT VALIDATION
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
})
    .AddFluentValidationAutoValidation(cfg =>
    {
        cfg.OverrideDefaultResultFactoryWith<FluentValidationFilter>();
    })
    .AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// MIDDLEWARES
app.UseExceptionHandler(x => { });
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
