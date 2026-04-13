using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.ErrorDictionary;
using BimManagerPortal.Shared.Dtos.ErrorDictionary;
using MediatR;

namespace BimManagerPortal.Application.Features.ErrorDictionary.Queries.GetErrorDictionaryEntries;

public record GetErrorDictionaryEntriesQuery : IRequest<List<ErrorDictionaryEntryDto>>;

internal class GetErrorDictionaryEntriesQueryHandler
    : IRequestHandler<GetErrorDictionaryEntriesQuery, List<ErrorDictionaryEntryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetErrorDictionaryEntriesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ErrorDictionaryEntryDto>> Handle(
        GetErrorDictionaryEntriesQuery request, CancellationToken ct)
    {
        var entries = await _unitOfWork.Repository<ErrorDictionaryEntry>().GetAllAsync();

        return entries
            .OrderBy(e => e.DictionaryType)
            .ThenBy(e => e.CreatedAt)
            .Select(e => new ErrorDictionaryEntryDto
            {
                Id = e.Id,
                DictionaryType = (int)e.DictionaryType,
                KeyPhrase = e.KeyPhrase,
                Explanation = e.Explanation,
                UserCreater = e.UserCreater,
                CreatedAt = e.CreatedAt.ToString("dd.MM.yyyy HH:mm")
            })
            .ToList();
    }
}
