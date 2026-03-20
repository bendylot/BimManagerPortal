using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Application.Other.PluginsConfigs.RestrictedAreas
{
    public class RestrictedAreaConfigEntity
    {
        [Required] 
        [StringLength(20, MinimumLength = 1,ErrorMessage = "Длина от 0 до 20 символов")]
        public string NameConfig { get; set; } = "";
        public int? Id { get; set; }
        [ValidateComplexType]
        public RestrictedAreaConfigProxy Data { get; set; } = new();
    }
}
