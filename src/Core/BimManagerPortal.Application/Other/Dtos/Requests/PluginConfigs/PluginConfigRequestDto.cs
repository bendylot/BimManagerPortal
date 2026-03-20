using System.Text.Json.Serialization;

namespace BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs
{
    public class PluginConfigRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("configuration")]
        public object Configuration { get; set; }
    }
}
