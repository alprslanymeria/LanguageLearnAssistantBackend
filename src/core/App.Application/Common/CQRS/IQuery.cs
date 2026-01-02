using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// MARKER INTERFACE FOR QUERIES THAT RETURN A VALUE.
/// </summary>
public interface IQuery<TResponse> : IRequest<ServiceResult<TResponse>> where TResponse : class
{
}
