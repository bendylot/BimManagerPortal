using System.Text.Json;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigData;

public record GetPluginBigDataQuery(GetPluginBigDataRequestDto getPluginBigDataRequestDto) : IRequest<GetPluginBigDataDto>;

public class GetPluginBigDataQueryHandler 
    : IRequestHandler<GetPluginBigDataQuery, GetPluginBigDataDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompressionService _compression;

    public GetPluginBigDataQueryHandler(
        IUnitOfWork unitOfWork,
        ICompressionService compression)
    {
        _unitOfWork = unitOfWork;
        _compression = compression;
    }

    public async Task<GetPluginBigDataDto> Handle(GetPluginBigDataQuery query,CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Repository<PluginBigData>()
            .GetByIdAsync(query.getPluginBigDataRequestDto.Id);
        
        try
        {
            var jsonBytes = _compression.Decompress(entity.JsonData);

            var json = JsonSerializer.Deserialize<JsonElement>(jsonBytes);

            var entityDto = new GetPluginBigDataDto(json);
            return entityDto;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}
