using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.WritingSessionRows.Dtos;

namespace App.Application.Features.WritingSessionRows.Commands.CreateWRows;

public record CreateWRowsCommand(SaveWritingRowsRequest Request) : ICommand<ServiceResult>;
