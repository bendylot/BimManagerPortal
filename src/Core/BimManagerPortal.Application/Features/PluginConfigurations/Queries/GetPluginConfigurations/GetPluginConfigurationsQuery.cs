using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.PluginConfigurations;
using BimManagerPortal.Shared.Other.PluginsConfigs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BimManagerPortal.Application.Features.PluginConfigurations.Queries.GetPluginConfigurations;

public record GetPluginConfigurationsQuery : IRequest<List<PluginConfigEntity>>;

internal class GetPluginConfigurationsQueryHandler : IRequestHandler<GetPluginConfigurationsQuery, List<PluginConfigEntity>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPluginConfigurationsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PluginConfigEntity>> Handle(GetPluginConfigurationsQuery query, CancellationToken ct)
    {
        var entities = await _unitOfWork.Repository<PluginConfiguration>()
            .Entities
            .ToListAsync(ct);

        return entities.Select(e => new PluginConfigEntity
        {
            Id         = e.Id.ToString(),
            Name       = e.Name,
            PluginName = e.PluginName,
            CreatedAt  = e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Data       = e.JsonData,
        }).ToList();
    }
}
