using BimManagerPortal.Domain.Common;

namespace BimManagerPortal.Domain.Entities.BigDataPlugins;

public class PluginBigData : BaseAuditableEntity
{
    protected PluginBigData()
    { }
    public PluginBigData(string UserCreater,
        string PluginName,
        string ConfigurationName,
        byte[] JsonData) : base(UserCreater)
    {
        this.PluginName = PluginName;
        this.ConfigurationName = ConfigurationName;
        this.JsonData = JsonData;
    }
    public string PluginName { get; set; }
    public string ConfigurationName { get; set; }
    public byte[] JsonData { get; set; } = null!;
    
}