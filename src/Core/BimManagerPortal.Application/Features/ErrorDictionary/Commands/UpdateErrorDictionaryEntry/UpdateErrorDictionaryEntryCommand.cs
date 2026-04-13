using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.ErrorDictionary;
using BimManagerPortal.Shared.Dtos.ErrorDictionary;
using MediatR;

namespace BimManagerPortal.Application.Features.ErrorDictionary.Commands.UpdateErrorDictionaryEntry;

public record UpdateErrorDictionaryEntryCommand(UpdateErrorDictionaryEntryRequestDto Dto) : IRequest;

internal class UpdateErrorDictionaryEntryCommandHandler : IRequestHandler<UpdateErrorDictionaryEntryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateErrorDictionaryEntryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateErrorDictionaryEntryCommand command, CancellationToken ct)
    {
        var dto = command.Dto;
        var entry = await _unitOfWork.Repository<ErrorDictionaryEntry>()
            .GetByIdAsync(dto.Id.ToString())
            ?? throw new KeyNotFoundException($"Запись с id '{dto.Id}' не найдена.");

        entry.KeyPhrase = dto.KeyPhrase;
        entry.Explanation = dto.Explanation;
        entry.UpdatedAt = DateTime.UtcNow;
        entry.UserUpdater = dto.UserUpdater;

        await _unitOfWork.Repository<ErrorDictionaryEntry>().UpdateAsync(entry);
        await _unitOfWork.SaveAsync(ct);
    }
}
