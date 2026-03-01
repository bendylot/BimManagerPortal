using System.Text.Json.Serialization;

namespace BimManagerPortal.Domain.Entities.Dtos.Requests.PluginConfigs
{
    public class PluginConfigRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("configuration")]
        public object Configuration { get; set; }
    }
}
