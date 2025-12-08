using App.API.ExceptionHandlers;
using App.API.Extensions;
using App.API.Filters;
using App.Application.Extensions;
using App.Infrastructure.Mapping;
using App.Persistence.Extensions;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);


// SERVICES
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services
    .AddCustomTokenAuth(builder.Configuration)
    .AddServices()
    .AddRepositories(builder.Configuration)
    .AddMapster(null, typeof(App.Application.ApplicationAssembly).Assembly);

// EXCEPTION HANDLERS
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// FLUENT VALIDATION
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddFluentValidationAutoValidation(cfg =>
{
    cfg.OverrideDefaultResultFactoryWith<FluentValidationFilter>();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// MIDDLEWARES
app.UseExceptionHandler(x => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();