using BimManagerPortal.Domain.Common;

namespace BimManagerPortal.Domain.Entities.BigDataPlugins;

public class PluginBigData : BaseAuditableEntity
{
    protected PluginBigData()
    { }
    public PluginBigData(string UserCreater, 
        string PluginName,
        byte[] JsonData) : base(UserCreater)
    {
        this.PluginName = PluginName;
        this.JsonData = JsonData;
    }
    public string PluginName { get; set; }
    public byte[] JsonData { get; set; } = null!;
    
}