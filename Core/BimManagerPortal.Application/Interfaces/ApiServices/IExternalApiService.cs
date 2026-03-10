using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Application.Other.Dtos.Responses.PluginConfigsDto;

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
