using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using BimManagerPortal.WebAssembly.Models.PluginReports;

namespace BimManagerPortal.WebAssembly.Services.PluginReports;

public interface IPluginReportProviderServiceProvider
{
    Task<List<GetAllPluginBigDatasDto>> GetConfigurations();

    Task<GetPluginBigDataDto?> GetConfiguration(string id);
}