using System.Net.Http.Json;
using System.Text.Json;
using BimManagerPortal.Shared.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Shared.Other.PluginsConfigs;

namespace BimManagerPortal.WebAssembly.Services.PluginConfigurations;

public class PluginConfigurationService : IPluginConfigurationService
{
    private readonly HttpClient _httpClient;

    public PluginConfigurationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PluginConfigEntity>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<PluginConfigEntity>>(
            "api/v1/public/plugin-configurations");
        return result ?? new List<PluginConfigEntity>();
    }

    public async Task CreateAsync(PluginConfigRequestDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/public/plugin-configurations", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(PluginConfigRequestDto dto, string id)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/v1/public/plugin-configurations/{id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/public/plugin-configurations/{id}");
        response.EnsureSuccessStatusCode();
    }
}
