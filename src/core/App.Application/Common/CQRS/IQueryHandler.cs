using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// HANDLER INTERFACE FOR CQRS QUERIES.
/// </summary>
/// <typeparam name="TQuery">THE TYPE OF QUERY TO HANDLE.</typeparam>
/// <typeparam name="TResponse">THE TYPE OF RESPONSE RETURNED BY THE QUERY.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;
