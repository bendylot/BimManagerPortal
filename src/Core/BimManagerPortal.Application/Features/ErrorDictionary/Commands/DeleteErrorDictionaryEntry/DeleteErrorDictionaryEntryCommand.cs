using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.ErrorDictionary;
using MediatR;

namespace BimManagerPortal.Application.Features.ErrorDictionary.Commands.DeleteErrorDictionaryEntry;

public record DeleteErrorDictionaryEntryCommand(string Id) : IRequest;

internal class DeleteErrorDictionaryEntryCommandHandler : IRequestHandler<DeleteErrorDictionaryEntryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteErrorDictionaryEntryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteErrorDictionaryEntryCommand command, CancellationToken ct)
    {
        var entry = await _unitOfWork.Repository<ErrorDictionaryEntry>().GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Запись с id '{command.Id}' не найдена.");

        await _unitOfWork.Repository<ErrorDictionaryEntry>().DeleteAsync(entry);
        await _unitOfWork.SaveAsync(ct);
    }
}
