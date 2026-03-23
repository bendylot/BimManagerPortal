using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.PluginConfigurations;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginConfigurations.Commands.DeletePluginConfiguration;

public record DeletePluginConfigurationCommand(string Id) : IRequest;

internal class DeletePluginConfigurationCommandHandler : IRequestHandler<DeletePluginConfigurationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePluginConfigurationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeletePluginConfigurationCommand command, CancellationToken ct)
    {
        var entity = await _unitOfWork.Repository<PluginConfiguration>().GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Конфигурация с id '{command.Id}' не найдена.");

        await _unitOfWork.Repository<PluginConfiguration>().DeleteAsync(entity);
        await _unitOfWork.SaveAsync(ct);
    }
}
