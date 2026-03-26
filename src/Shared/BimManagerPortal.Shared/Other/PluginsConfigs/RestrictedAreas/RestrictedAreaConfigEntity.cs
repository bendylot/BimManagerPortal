using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Shared.Other.PluginsConfigs.RestrictedAreas
{
    public class RestrictedAreaConfigEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 1,ErrorMessage = "Длина от 1 до 100 символов")]
        public string NameConfig { get; set; } = "";
        public string? Id { get; set; }
        [ValidateComplexType]
        public RestrictedAreaConfigProxy Data { get; set; } = new();
    }
}
