using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.ErrorDictionary;
using BimManagerPortal.Shared.Dtos.ErrorDictionary;
using MediatR;

namespace BimManagerPortal.Application.Features.ErrorDictionary.Commands.CreateErrorDictionaryEntry;

public record CreateErrorDictionaryEntryCommand(CreateErrorDictionaryEntryRequestDto Dto) : IRequest;

internal class CreateErrorDictionaryEntryCommandHandler : IRequestHandler<CreateErrorDictionaryEntryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateErrorDictionaryEntryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateErrorDictionaryEntryCommand command, CancellationToken ct)
    {
        var dto = command.Dto;
        var entry = new ErrorDictionaryEntry(
            dto.UserCreater,
            (ErrorDictionaryType)dto.DictionaryType,
            dto.KeyPhrase,
            dto.Explanation);

        await _unitOfWork.Repository<ErrorDictionaryEntry>().AddAsync(entry);
        await _unitOfWork.SaveAsync(ct);
    }
}
