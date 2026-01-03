using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// MARKER INTERFACE FOR CQRS QUERIES.
/// QUERIES ARE READ OPERATIONS THAT DO NOT MODIFY STATE.
/// </summary>
/// <typeparam name="TResponse">THE TYPE OF RESPONSE RETURNED BY THE QUERY.</typeparam>
public interface IQuery<TResponse> : IRequest<TResponse>;
