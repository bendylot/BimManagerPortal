using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BimManagerPortal.Application.Interfaces.ApiServices;
using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Application.Other.Dtos.Responses.PluginConfigsDto;
using BimManagerPortal.Application.Other.Exceptions;

namespace BimManagerPortal.Persistance.ApiServices
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        /*private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true // опционально, для красоты в БД
        };*/
        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendPluginConfigAsync(PluginConfigRequestDto dto)
        {
            var _options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true // опционально, для красоты в БД
            };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/public/plugin-configurations", dto, _options);

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

        public async Task UpdateExistPluginConfigAsync(PluginConfigRequestDto dto, int id)
        {
            var _options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true // опционально, для красоты в БД
            };
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/public/plugin-configurations/{id}", dto, _options);

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

        public async Task DeletePluginConfigAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/public/plugin-configurations/{id}");

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
                throw new Exception();
                /*throw new SendPluginConfigException(
                    message: $"API Error: {response.StatusCode}",
                    statusCode: response.StatusCode,
                    detail: detail,
                    userFriendlyMessage: response.StatusCode == System.Net.HttpStatusCode.InternalServerError
                        ? detail // В вашем случае (500) выводим именно текст из detail
                        : "Не удалось сохранить конфигурацию"
                );*/
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
