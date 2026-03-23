using BimManagerPortal.Domain.Common;

namespace BimManagerPortal.Domain.Entities.PluginConfigurations;

public class PluginConfiguration : BaseAuditableEntity
{
    protected PluginConfiguration() { }

    public PluginConfiguration(string name, string pluginName, string jsonData) : base("system")
    {
        Name = name;
        PluginName = pluginName;
        JsonData = jsonData;
    }

    public string Name { get; set; } = null!;
    public string PluginName { get; set; } = null!;
    public string JsonData { get; set; } = null!;
}
