using BimManagerPortal.Domain.Entities.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Entities.Dtos.Responses.PluginConfigsDto;

namespace BimManagerPortal.WebBlazorSite.Services.ExternalApiService
{
    public interface IExternalApiService
    {
        Task DeletePluginConfigAsync(int id);
        Task SendPluginConfigAsync(PluginConfigRequestDto dto);
        Task UpdateExistPluginConfigAsync(PluginConfigRequestDto dto, int id);
        Task<PluginConfigsResponseDto> GetPluginConfigAsync();
    }
}
