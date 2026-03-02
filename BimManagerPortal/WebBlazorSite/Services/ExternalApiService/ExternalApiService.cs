using System.Text.Json;
using BimManagerPortal.Domain.Entities.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Entities.Dtos.Responses.PluginConfigsDto;
using BimManagerPortal.WebBlazorSite.Exceptions;

namespace BimManagerPortal.WebBlazorSite.Services.ExternalApiService
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendPluginConfigAsync(PluginConfigRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/public/plugin-configurations", dto);

            if (!response.IsSuccessStatusCode)
            {
                string? detail = null;
                try 
                {
                    // Пытаемся распарсить Problem Details JSON
                    var problemDetails = await response.Content.ReadFromJsonAsync<System.Text.Json.Nodes.JsonObject>();
                    detail = problemDetails?["detail"]?.ToString();
                }
                catch 
                {
                    // Если это не JSON, просто читаем как строку
                    detail = await response.Content.ReadAsStringAsync();
                }

                throw new SendPluginConfigException(
                    message: $"API Error: {response.StatusCode}",
                    statusCode: response.StatusCode,
                    detail: detail,
                    userFriendlyMessage: response.StatusCode == System.Net.HttpStatusCode.InternalServerError 
                        ? detail // В вашем случае (500) выводим именно текст из detail
                        : "Не удалось сохранить конфигурацию"
                );
            }
        }
        public async Task<PluginConfigsResponseDto> GetPluginConfigAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/public/plugin-configurations");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {error}");
            }
            var stream = await response.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<PluginConfigsResponseDto>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
                throw new InvalidOperationException("Failed to deserialize PluginConfigsResponseDto.");

            return result;
        }
    }
}
