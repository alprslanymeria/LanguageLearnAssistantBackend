using MediatR;

namespace App.Application.Common.CQRS;

/// <summary>
/// MARKER INTERFACE FOR COMMANDS THAT DO NOT RETURN A VALUE.
/// </summary>
public interface ICommand : IRequest<ServiceResult>
{
}

/// <summary>
/// MARKER INTERFACE FOR COMMANDS THAT RETURN A VALUE.
/// </summary>
public interface ICommand<TResponse> : IRequest<ServiceResult<TResponse>> where TResponse : class
{
}
