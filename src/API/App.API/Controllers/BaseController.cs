using App.Application;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Handles responses for operations that return ServiceResult with data.
    /// Returns Ok for success, BadRequest for failure.
    /// </summary>
    protected IActionResult HandleResult<T>(ServiceResult<T> result) where T : class
    {
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Handles responses based on HttpStatusCode in ServiceResult.
    /// Supports OK, NotFound, and BadRequest status codes.
    /// </summary>
    protected IActionResult HandleResultWithStatus<T>(ServiceResult<T> result) where T : class
    {
        return result.Status switch
        {
            HttpStatusCode.OK => Ok(result),
            HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    /// <summary>
    /// Handles responses for Create operations.
    /// Returns Created with location for success, BadRequest for failure.
    /// </summary>
    protected IActionResult HandleCreateResult<T>(ServiceResult<T> result) where T : class
    {
        return result.IsSuccess ? Created(result.UrlAsCreated!, result) : BadRequest(result);
    }

    /// <summary>
    /// Handles responses for Delete operations.
    /// Returns NoContent for success, NotFound if not found, BadRequest otherwise.
    /// </summary>
    protected IActionResult HandleDeleteResult(ServiceResult result)
    {
        return result.Status switch
        {
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.NotFound => NotFound(result),
            _ => BadRequest(result)
        };
    }

    /// <summary>
    /// Validates that the route id matches the dto id.
    /// Returns BadRequest if they don't match, null otherwise.
    /// </summary>
    protected IActionResult? ValidateIdMatch(int routeId, int dtoId)
    {
        if (routeId != dtoId)
        {
            return BadRequest(new { ErrorMessage = new[] { "Id mismatch" } });
        }
        return null;
    }
}
