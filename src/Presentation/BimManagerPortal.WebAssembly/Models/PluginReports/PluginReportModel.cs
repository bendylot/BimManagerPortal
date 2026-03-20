using System.Text.Json;

namespace BimManagerPortal.WebAssembly.Models.PluginReports;

public class PluginReportModel
{
    public string Id { get; set; }
    public string PluginName { get; set; }
    public JsonElement JsonData { get; set; }
    public string UserCreater { get; set; }
}