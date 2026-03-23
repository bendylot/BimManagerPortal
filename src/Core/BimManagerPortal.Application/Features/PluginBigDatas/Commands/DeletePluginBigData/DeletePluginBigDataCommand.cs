using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Commands.DeletePluginBigData;

public record DeletePluginBigDataCommand(string Id) : IRequest;

internal class DeletePluginBigDataCommandHandler : IRequestHandler<DeletePluginBigDataCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePluginBigDataCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeletePluginBigDataCommand command, CancellationToken ct)
    {
        var entity = await _unitOfWork.Repository<PluginBigData>().GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Конфигурация с id '{command.Id}' не найдена.");

        await _unitOfWork.Repository<PluginBigData>().DeleteAsync(entity);
        await _unitOfWork.SaveAsync(ct);
    }
}
