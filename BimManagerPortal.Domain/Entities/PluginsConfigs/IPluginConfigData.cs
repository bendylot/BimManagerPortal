using BimManagerPortal.Domain.Entities.PluginsConfigs.RestrictedAreas;
using System.Text.Json.Serialization;

namespace BimManagerPortal.Domain.Entities.PluginsConfigs
{
    [JsonDerivedType(typeof(RestrictedAreaConfigProxy))]
    public interface IPluginConfigData
    {
        
    }
}
