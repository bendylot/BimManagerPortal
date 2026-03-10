using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Application.Other.PluginsConfigs
{
    public class PluginConfigEntity
    {
        [Required(ErrorMessage = "Поле обязательно")]
        [StringLength(20, MinimumLength = 1,ErrorMessage = "Длина от 1 до 20 символов")]
        public string Name { get; set; }
        public int? Id { get; set; }
        public object Data { get; set; }
    }

}
