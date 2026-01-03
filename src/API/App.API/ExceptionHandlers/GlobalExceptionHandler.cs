using App.Application.Common;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace App.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var errorAsDto = ServiceResult.Fail(exception.Message, HttpStatusCode.InternalServerError);

        httpContext.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(errorAsDto, cancellationToken);

        return true;

        // return true;  --> I'VE HANDLED THIS ERROR AND I WILL RETURN THE RELEVANT RESPONSE MODEL.
        // return false; --> I'VE HANDLED THIS ERROR, PERFORMED THE NECESSARY OPERATIONS. LET IT CONTINUE ON ITS JOURNEY.
    }
}
