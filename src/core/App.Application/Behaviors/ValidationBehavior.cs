using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace App.Application.Behaviors;

/// <summary>
/// PIPELINE BEHAVIOR FOR VALIDATING REQUESTS USING FLUENTVALIDATION.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        logger.LogInformation("ValidationBehavior: VALIDATING REQUEST {RequestType}", typeof(TRequest).Name);

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errorMessages = failures.Select(f => f.ErrorMessage).ToList();
            logger.LogWarning("ValidationBehavior: VALIDATION FAILED FOR {RequestType}. ERRORS: {Errors}",
                typeof(TRequest).Name,
                string.Join(", ", errorMessages));

            // RETURN A SERVICERESULT WITH VALIDATION ERRORS
            var resultType = typeof(TResponse);
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Common.ServiceResult<>))
            {
                var dataType = resultType.GetGenericArguments()[0];
                var failMethod = resultType.GetMethod(nameof(Common.ServiceResult<object>.Fail),
                    new[] { typeof(List<string>), typeof(HttpStatusCode) });

                if (failMethod != null)
                {
                    var result = failMethod.Invoke(null, new object[] { errorMessages, HttpStatusCode.BadRequest });
                    return (TResponse)result!;
                }
            }
            else if (resultType == typeof(Common.ServiceResult))
            {
                var failMethod = resultType.GetMethod(nameof(Common.ServiceResult.Fail),
                    new[] { typeof(List<string>), typeof(HttpStatusCode) });

                if (failMethod != null)
                {
                    var result = failMethod.Invoke(null, new object[] { errorMessages, HttpStatusCode.BadRequest });
                    return (TResponse)result!;
                }
            }
        }

        logger.LogInformation("ValidationBehavior: VALIDATION SUCCEEDED FOR {RequestType}", typeof(TRequest).Name);
        return await next();
    }
}
