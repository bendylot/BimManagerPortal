namespace BimManagerPortal.Domain.Entities.BigDataPlugins;

public class RestrictedAreaPluginBigData : PluginBigData
{
    public RestrictedAreaPluginBigData(string UserCreater, string PluginName, string ConfigurationName, byte[] JsonData) : base(UserCreater, PluginName, ConfigurationName, JsonData)
    {
    }
}