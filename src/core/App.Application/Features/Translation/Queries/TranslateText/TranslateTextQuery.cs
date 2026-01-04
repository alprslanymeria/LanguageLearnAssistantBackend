using App.Application.Common;
using App.Application.Common.CQRS;
using App.Application.Features.Translation.Dtos;

namespace App.Application.Features.Translation.Queries.TranslateText;

public record TranslateTextQuery(

    TranslateTextRequest Request,
    string UserId,
    string AccessToken,
    CancellationToken CancellationToken = default) : IQuery<ServiceResult<TranslateTextResponse>>;
