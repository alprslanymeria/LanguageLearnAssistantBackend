using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ListeningSessionRows.Dtos;

namespace App.Application.Features.ListeningSessionRows.Commands.CreateLRows;

public record CreateLRowsCommand(SaveListeningRowsRequest Request) : ICommand<ServiceResult>;
