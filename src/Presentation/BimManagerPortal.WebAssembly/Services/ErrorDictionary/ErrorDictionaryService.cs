using System.Net.Http.Json;
using BimManagerPortal.Shared.Dtos.ErrorDictionary;

namespace BimManagerPortal.WebAssembly.Services.ErrorDictionary;

public class ErrorDictionaryService : IErrorDictionaryService
{
    private readonly HttpClient _httpClient;

    public ErrorDictionaryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ErrorDictionaryEntryDto>> GetAll()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ErrorDictionaryEntryDto>>(
            "api/v1/public/error-dictionary");
        return result ?? new List<ErrorDictionaryEntryDto>();
    }

    public async Task Create(CreateErrorDictionaryEntryRequestDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/public/error-dictionary", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(UpdateErrorDictionaryEntryRequestDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/v1/public/error-dictionary", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/public/error-dictionary/{id}");
        response.EnsureSuccessStatusCode();
    }
}
