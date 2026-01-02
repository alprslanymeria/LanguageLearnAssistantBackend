using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// HANDLER INTERFACE FOR QUERIES THAT RETURN A VALUE.
/// </summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ServiceResult<TResponse>>
    where TQuery : IQuery<TResponse>
    where TResponse : class
{
}
