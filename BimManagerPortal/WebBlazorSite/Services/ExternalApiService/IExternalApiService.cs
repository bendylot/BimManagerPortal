using BimManagerPortal.Domain.Entities.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Entities.Dtos.Responses.PluginConfigsDto;

namespace BimManagerPortal.WebBlazorSite.Services.ExternalApiService
{
    public interface IExternalApiService
    {
        Task SendPluginConfigAsync(PluginConfigRequestDto dto);
        Task<PluginConfigsResponseDto> GetPluginConfigAsync();
    }
}
