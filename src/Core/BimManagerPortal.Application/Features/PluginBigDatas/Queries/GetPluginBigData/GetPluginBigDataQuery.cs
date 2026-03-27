using System.Text.Json;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigData;

public record GetPluginBigDataQuery(GetPluginBigDataRequestDto getPluginBigDataRequestDto) : IRequest<GetPluginBigDataResponseDto>;

public class GetPluginBigDataQueryHandler
    : IRequestHandler<GetPluginBigDataQuery, GetPluginBigDataResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompressionService _compression;
    private readonly ILogger<GetPluginBigDataQueryHandler> _logger;

    public GetPluginBigDataQueryHandler(
        IUnitOfWork unitOfWork,
        ICompressionService compression,
        ILogger<GetPluginBigDataQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _compression = compression;
        _logger = logger;
    }

    public async Task<GetPluginBigDataResponseDto> Handle(GetPluginBigDataQuery query, CancellationToken cancellationToken)
    {
        var id = query.getPluginBigDataRequestDto.Id;

        var entity = await _unitOfWork.Repository<PluginBigData>()
            .GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Запись с id '{id}' не найдена.");

        try
        {
            var jsonBytes = _compression.Decompress(entity.JsonData);
            var json = JsonSerializer.Deserialize<JsonElement>(jsonBytes);
            return new GetPluginBigDataResponseDto(json);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to decompress data for record {Id}", id);
            throw;
        }
    }
}
