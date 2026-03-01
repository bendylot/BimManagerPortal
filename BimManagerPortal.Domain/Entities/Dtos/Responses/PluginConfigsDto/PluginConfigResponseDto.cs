using System.Text.Json.Serialization;

namespace BimManagerPortal.Domain.Entities.Dtos.Responses.PluginConfigsDto
{
    public class PluginConfigResponseDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("configuration")]
        public object Configuration { get; set; }
    }
}
