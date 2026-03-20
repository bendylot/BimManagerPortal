using System.Text.Json.Serialization;
using BimManagerPortal.Application.Other.PluginsConfigs.RestrictedAreas;

namespace BimManagerPortal.Application.Other.PluginsConfigs
{
    [JsonDerivedType(typeof(RestrictedAreaConfigProxy))]
    public interface IPluginConfigData
    {
        
    }
}
