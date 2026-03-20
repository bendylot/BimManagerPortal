using System.Text.Json;
using BimManagerPortal.WebAssembly.Models.BuiltIntTab;

namespace BimManagerPortal.WebAssembly.Models.Results;

public class ReadPluginReportResult
{
    public ReadPluginReportResult(JsonElement PluginReportModel, string PluginName)
    {
        this.PluginReportModel = PluginReportModel;
        this.PluginName = PluginName;
    }
    public BuiltIntTabModel BuiltIntTabModel { get; private set; }
    public JsonElement PluginReportModel { get; private set; }
    public string PluginName { get; private set; }
}