using System.Net.Http.Json;
using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using BimManagerPortal.WebAssembly.Models.PluginReports;

namespace BimManagerPortal.WebAssembly.Services.PluginReports;

public class PluginReportProviderServiceProvider : IPluginReportProviderServiceProvider
{
    private readonly HttpClient _httpClient;

    public PluginReportProviderServiceProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<GetAllPluginBigDatasDto>> GetConfigurations()
    {
        var result = await _httpClient.GetFromJsonAsync<List<GetAllPluginBigDatasDto>>(
            "api/v1/public/plugin-big-data/");

        return result ?? new List<GetAllPluginBigDatasDto>();
    }

    public async Task<PagedResultDto<GetAllPluginBigDatasDto>> GetConfigurationsPaged(
        int page, int pageSize, string? search, string? sortColumn, bool sortAscending)
    {
        var url = $"api/v1/public/plugin-big-data/paged?page={page}&pageSize={pageSize}&sortAscending={sortAscending}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrWhiteSpace(sortColumn))
            url += $"&sortColumn={sortColumn}";

        var result = await _httpClient.GetFromJsonAsync<PagedResultDto<GetAllPluginBigDatasDto>>(url);
        return result ?? new PagedResultDto<GetAllPluginBigDatasDto>();
    }

    public async Task<GetPluginBigDataResponseDto?> GetConfiguration(string id)
    {
        return await _httpClient.GetFromJsonAsync<GetPluginBigDataResponseDto>($"api/v1/public/plugin-big-data/{id}");
    }

    public async Task DeleteConfiguration(string id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/public/plugin-big-data/{id}");
        response.EnsureSuccessStatusCode();
    }

}