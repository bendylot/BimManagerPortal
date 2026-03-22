using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;

public record GetPluginBigDatasQuery : IRequest<List<GetAllPluginBigDatasDto>>;

public class GetPluginBigDatasQueryHandler
    : IRequestHandler<GetPluginBigDatasQuery, List<GetAllPluginBigDatasDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompressionService _compression;
    private readonly ILogger<GetPluginBigDatasQueryHandler> _logger;

    public GetPluginBigDatasQueryHandler(
        IUnitOfWork unitOfWork,
        ICompressionService compression,
        ILogger<GetPluginBigDatasQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _compression = compression;
        _logger = logger;
    }

    public async Task<List<GetAllPluginBigDatasDto>> Handle(GetPluginBigDatasQuery query, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Repository<PluginBigData>()
            .Entities
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Found {Count} plugin big data records", entities.Count);

        var list = new List<GetAllPluginBigDatasDto>();
        foreach (var e in entities)
        {
            try
            {
                _compression.Decompress(e.JsonData);

                var entity = new GetAllPluginBigDatasDto(
                    e.Id.ToString(),
                    e.PluginName,
                    e.UserCreater,
                    e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                list.Add(entity);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to decompress data for record {Id}", e.Id);
            }
        }

        return list;
    }
}
