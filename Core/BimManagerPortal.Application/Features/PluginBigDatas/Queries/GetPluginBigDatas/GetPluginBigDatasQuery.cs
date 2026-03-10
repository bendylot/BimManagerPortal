using System.Text.Json;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;

public record GetPluginBigDatasQuery : IRequest<List<GetAllPluginBigDatasDto>>;

public class GetPluginBigDatasQueryHandler 
    : IRequestHandler<GetPluginBigDatasQuery, List<GetAllPluginBigDatasDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompressionService _compression;

    public GetPluginBigDatasQueryHandler(
        IUnitOfWork unitOfWork,
        ICompressionService compression)
    {
        _unitOfWork = unitOfWork;
        _compression = compression;
    }

    public async Task<List<GetAllPluginBigDatasDto>> Handle(GetPluginBigDatasQuery query, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.Repository<PluginBigData>()
            .Entities
            .ToListAsync(cancellationToken);
        var list = new List<GetAllPluginBigDatasDto>();
        foreach( var e in  entities)
        {
            try
            {
                var jsonBytes = _compression.Decompress(e.JsonData);

                //var json = JsonSerializer.Deserialize<JsonElement>(jsonBytes);

                var entity = new GetAllPluginBigDatasDto(
                    e.Id.ToString(),
                    e.PluginName,
                    //json,
                    e.UserCreater,
                    e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
                list.Add(entity);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        return list;
    }
}
