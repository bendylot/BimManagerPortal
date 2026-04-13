using BimManagerPortal.Shared.Dtos.ErrorDictionary;

namespace BimManagerPortal.WebAssembly.Services.ErrorDictionary;

public interface IErrorDictionaryService
{
    Task<List<ErrorDictionaryEntryDto>> GetAll();
    Task Create(CreateErrorDictionaryEntryRequestDto dto);
    Task Update(UpdateErrorDictionaryEntryRequestDto dto);
    Task Delete(Guid id);
}
