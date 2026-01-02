using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// HANDLER INTERFACE FOR COMMANDS THAT DO NOT RETURN A VALUE.
/// </summary>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ServiceResult>
    where TCommand : ICommand
{
}

/// <summary>
/// HANDLER INTERFACE FOR COMMANDS THAT RETURN A VALUE.
/// </summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ServiceResult<TResponse>>
    where TCommand : ICommand<TResponse>
    where TResponse : class
{
}
