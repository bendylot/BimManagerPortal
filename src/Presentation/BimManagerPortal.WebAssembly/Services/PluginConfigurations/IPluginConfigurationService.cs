using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Application.Other.PluginsConfigs;

namespace BimManagerPortal.WebAssembly.Services.PluginConfigurations;

public interface IPluginConfigurationService
{
    Task<List<PluginConfigEntity>> GetAllAsync();
    Task CreateAsync(PluginConfigRequestDto dto);
    Task UpdateAsync(PluginConfigRequestDto dto, string id);
    Task DeleteAsync(string id);
}
