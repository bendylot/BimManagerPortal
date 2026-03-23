using BimManagerPortal.Domain.Entities.Plugins;
using BimManagerPortal.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Components.PluginConfigurations.Tabs.CreateConfigurations;

public partial class CreateConfiguration
{
    [Parameter] public EventCallback OnCreated { get; set; }
    protected List<AppType> AvailableApps = Enum.GetValues<AppType>().ToList();
    protected List<PluginEntity> AvailablePlugins = new();
    protected AppType? SelectedApp;
    protected PluginEntity? SelectedPlugin;

    private Dictionary<AppType, List<PluginEntity>> _pluginMap = new();

    protected override void OnInitialized()
    {
        _pluginMap = BuildPluginMap();
    }

    protected void SelectApp(AppType app)
    {
        SelectedApp = app;
        SelectedPlugin = null;
        AvailablePlugins = _pluginMap.TryGetValue(app, out var plugins) ? plugins : new();
    }

    protected void SelectPlugin(PluginEntity plugin)
    {
        SelectedPlugin = plugin;
    }

    private Dictionary<AppType, List<PluginEntity>> BuildPluginMap()
    {
        var allPlugins = new List<PluginEntity>
        {
            new("RestrictedArea",  AppType.Revit,      "SOFTAPRO"),
            new("ClashDefender",   AppType.Navisworks, "SOFTAPRO"),
            new("ElementFinder",   AppType.Revit,      "SOFTAPRO"),
            new("PluginAutoCad1",  AppType.AutoCAD,    "Company1"),
            new("PluginCivil1",    AppType.Civil3D,    "Company1"),
        };

        var dict = new Dictionary<AppType, List<PluginEntity>>();
        foreach (var appType in AvailableApps)
        {
            dict[appType] = allPlugins.Where(p => p.App == appType).ToList();
        }
        return dict;
    }
}
