using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using BimManagerPortal.WebAssembly.Models.PluginReports;

namespace BimManagerPortal.WebAssembly.Services.PluginReports;

public interface IPluginReportProviderServiceProvider
{
    Task<List<GetAllPluginBigDatasDto>> GetConfigurations();

    Task<PagedResultDto<GetAllPluginBigDatasDto>> GetConfigurationsPaged(
        int page, int pageSize, string? search, string? sortColumn, bool sortAscending);

    Task<GetPluginBigDataResponseDto?> GetConfiguration(string id);

    Task DeleteConfiguration(string id);
}