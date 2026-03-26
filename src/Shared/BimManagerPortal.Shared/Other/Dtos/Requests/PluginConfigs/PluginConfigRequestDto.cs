using System.Text.Json.Serialization;

namespace BimManagerPortal.Shared.Other.Dtos.Requests.PluginConfigs
{
    public class PluginConfigRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("pluginName")]
        public string? PluginName { get; set; }
        [JsonPropertyName("configuration")]
        public object Configuration { get; set; }
    }
}
