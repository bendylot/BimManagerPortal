using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.PluginConfigurations;
using BimManagerPortal.Shared.Other.PluginsConfigs;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginConfigurations.Queries.GetPluginConfiguration;

public record GetPluginConfigurationQuery(string Id) : IRequest<PluginConfigEntity>;

internal class GetPluginConfigurationQueryHandler : IRequestHandler<GetPluginConfigurationQuery, PluginConfigEntity>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPluginConfigurationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PluginConfigEntity> Handle(GetPluginConfigurationQuery query, CancellationToken ct)
    {
        var entity = await _unitOfWork.Repository<PluginConfiguration>().GetByIdAsync(query.Id)
            ?? throw new KeyNotFoundException($"Конфигурация с id '{query.Id}' не найдена.");

        return new PluginConfigEntity
        {
            Id   = entity.Id.ToString(),
            Name = entity.Name,
            Data = entity.JsonData,
        };
    }
}
