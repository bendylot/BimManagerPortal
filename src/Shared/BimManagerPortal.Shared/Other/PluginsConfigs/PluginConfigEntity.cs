using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BimManagerPortal.Shared.Other.PluginsConfigs
{
    public class PluginConfigEntity
    {
        [Required(ErrorMessage = "Поле обязательно")]
        [StringLength(20, MinimumLength = 1,ErrorMessage = "Длина от 1 до 20 символов")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("pluginName")]
        public string? PluginName { get; set; }

        [JsonPropertyName("createdAt")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }

}
