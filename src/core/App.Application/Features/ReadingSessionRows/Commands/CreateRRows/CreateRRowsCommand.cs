using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.ReadingSessionRows.Dtos;

namespace App.Application.Features.ReadingSessionRows.Commands.CreateRRows;

public record CreateRRowsCommand(SaveReadingRowsRequest Request) : ICommand<ServiceResult>;
