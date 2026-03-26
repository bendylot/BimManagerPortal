using System.Text.Json.Serialization;
using BimManagerPortal.Shared.Other.PluginsConfigs.RestrictedAreas;

namespace BimManagerPortal.Shared.Other.PluginsConfigs
{
    [JsonDerivedType(typeof(RestrictedAreaConfigProxy))]
    public interface IPluginConfigData
    {

    }
}
