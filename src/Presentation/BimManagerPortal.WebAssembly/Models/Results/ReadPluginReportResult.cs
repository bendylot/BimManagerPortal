using System.Text.Json;
using BimManagerPortal.WebAssembly.Models.BuiltIntTab;

namespace BimManagerPortal.WebAssembly.Models.Results;

public class ReadPluginReportResult
{
    public ReadPluginReportResult(string id, JsonElement pluginReportModel, string pluginName)
    {
        Id = id;
        PluginReportModel = pluginReportModel;
        PluginName = pluginName;
    }
    public string Id { get; private set; }
    public BuiltIntTabModel BuiltIntTabModel { get; private set; }
    public JsonElement PluginReportModel { get; private set; }
    public string PluginName { get; private set; }
}