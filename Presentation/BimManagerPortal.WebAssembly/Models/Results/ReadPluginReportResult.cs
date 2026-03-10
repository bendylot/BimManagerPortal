using BimManagerPortal.WebAssembly.Models.BuiltIntTab;

namespace BimManagerPortal.WebAssembly.Models.Results;

public class ReadPluginReportResult
{
    public ReadPluginReportResult(object PluginReportModel)
    {
        this.PluginReportModel = PluginReportModel;
    }
    public BuiltIntTabModel BuiltIntTabModel { get; private set; }
    public object PluginReportModel { get; private set; }
}