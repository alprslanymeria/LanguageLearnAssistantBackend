using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// HANDLER INTERFACE FOR CQRS COMMANDS THAT RETURN A RESPONSE.
/// </summary>
/// <typeparam name="TCommand">THE TYPE OF COMMAND TO HANDLE.</typeparam>
/// <typeparam name="TResponse">THE TYPE OF RESPONSE RETURNED BY THE COMMAND.</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;

/// <summary>
/// HANDLER INTERFACE FOR CQRS COMMANDS THAT DO NOT RETURN A RESPONSE.
/// </summary>
/// <typeparam name="TCommand">THE TYPE OF COMMAND TO HANDLE.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand;
