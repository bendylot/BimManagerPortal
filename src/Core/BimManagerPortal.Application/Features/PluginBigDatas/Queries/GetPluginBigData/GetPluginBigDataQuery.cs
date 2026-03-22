using System.Text.Json;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        var entity = await _unitOfWork.Repository<PluginBigData>()
            .GetByIdAsync(query.getPluginBigDataRequestDto.Id);

        try
        {
            var jsonBytes = _compression.Decompress(entity.JsonData);

            var json = JsonSerializer.Deserialize<JsonElement>(jsonBytes);

            var entityDto = new GetPluginBigDataResponseDto(json);
            return entityDto;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to decompress/deserialize PluginBigData {Id}", query.getPluginBigDataRequestDto.Id);
            throw;
        }
    }
}
