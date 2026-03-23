using System.Text.Json;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Entities.PluginConfigurations;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginConfigurations.Commands.PostPluginConfiguration;

public record PostPluginConfigurationCommand(PluginConfigRequestDto Dto) : IRequest;

internal class PostPluginConfigurationCommandHandler : IRequestHandler<PostPluginConfigurationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PostPluginConfigurationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PostPluginConfigurationCommand command, CancellationToken ct)
    {
        var dto = command.Dto;
        var jsonData = JsonSerializer.Serialize(dto.Configuration);

        var entity = new PluginConfiguration(
            dto.Name,
            dto.PluginName ?? string.Empty,
            jsonData);

        await _unitOfWork.Repository<PluginConfiguration>().AddAsync(entity);
        await _unitOfWork.SaveAsync(ct);
    }
}
