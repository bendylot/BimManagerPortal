using BimManagerPortal.Shared.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Shared.Other.Dtos.Responses.PluginConfigsDto;

namespace BimManagerPortal.Application.Interfaces.ApiServices
{
    public interface IExternalApiService
    {
        Task DeletePluginConfigAsync(int id);
        Task SendPluginConfigAsync(PluginConfigRequestDto dto);
        Task UpdateExistPluginConfigAsync(PluginConfigRequestDto dto, int id);
        Task<PluginConfigsResponseDto> GetPluginConfigAsync();
    }
}
