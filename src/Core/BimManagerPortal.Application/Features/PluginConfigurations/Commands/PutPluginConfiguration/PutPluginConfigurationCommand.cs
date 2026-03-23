using System.Text.Json;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Common;
using BimManagerPortal.Domain.Entities.PluginConfigurations;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginConfigurations.Commands.PutPluginConfiguration;

public record PutPluginConfigurationCommand(string Id, PluginConfigRequestDto Dto) : IRequest;

internal class PutPluginConfigurationCommandHandler : IRequestHandler<PutPluginConfigurationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PutPluginConfigurationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PutPluginConfigurationCommand command, CancellationToken ct)
    {
        var entity = await _unitOfWork.Repository<PluginConfiguration>().GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Конфигурация с id '{command.Id}' не найдена.");

        entity.Name    = command.Dto.Name;
        entity.JsonData = JsonSerializer.Serialize(command.Dto.Configuration);
        entity.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

        await _unitOfWork.Repository<PluginConfiguration>().UpdateAsync(entity);
        await _unitOfWork.SaveAsync(ct);
    }
}
