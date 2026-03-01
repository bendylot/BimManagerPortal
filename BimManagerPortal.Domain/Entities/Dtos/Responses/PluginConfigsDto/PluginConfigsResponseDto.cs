using System.Text.Json.Serialization;

namespace BimManagerPortal.Domain.Entities.Dtos.Responses.PluginConfigsDto;

public class PluginConfigsResponseDto
{
    [JsonPropertyName("status")]
    public int Status { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("data")]
    public List<PluginConfigResponseDto> Data { get; set; }
}