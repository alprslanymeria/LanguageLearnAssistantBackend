using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// MARKER INTERFACE FOR CQRS COMMANDS THAT RETURN A RESPONSE.
/// COMMANDS ARE WRITE OPERATIONS THAT MODIFY STATE.
/// </summary>
/// <typeparam name="TResponse">THE TYPE OF RESPONSE RETURNED BY THE COMMAND.</typeparam>
public interface ICommand<TResponse> : IRequest<TResponse>;

/// <summary>
/// MARKER INTERFACE FOR CQRS COMMANDS THAT DO NOT RETURN A RESPONSE.
/// COMMANDS ARE WRITE OPERATIONS THAT MODIFY STATE.
/// </summary>
public interface ICommand : IRequest;
