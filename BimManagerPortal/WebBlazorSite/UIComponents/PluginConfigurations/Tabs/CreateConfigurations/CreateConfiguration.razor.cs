using BimManagerPortal.Domain.Entities.AppTypes;
using BimManagerPortal.Domain.Entities.Plugins;

// [cite: 4]


namespace BimManagerPortal.WebBlazorSite.UIComponents.PluginConfigurations.Tabs.CreateConfigurations
{
    public partial class CreateConfiguration
    {
        #region protected
        // Вызывается автоматически при создании компонента
        protected override void OnInitialized()
        {
            _pluginMap = LoadDictionaryPlugins();
        }
        protected List<AppType> AvailableApps = Enum.GetValues<AppType>().ToList();
        protected List<PluginEntity> AvailablePlugins = new();

        protected AppType? SelectedApp;
        protected PluginEntity? SelectedPlugin;

        protected void SelectApp(AppType app)
        {
            SelectedApp = app;
            SelectedPlugin = null;
            if (_pluginMap.TryGetValue(app, out var plugins))
            {
                AvailablePlugins = plugins;
            }
        }
        protected void SelectPlugin(PluginEntity plugin)
        {
            SelectedPlugin = plugin;
            if (SelectedPlugin.Name=="RestrictedArea")
            {
                //PluginConfigEntity.Data = new RestrictedAreaConfigProxy();
            }
        }
        #endregion

        #region private
        
        private IEnumerable<PluginEntity> _pluginEntities => LoadPlugins();
        private Dictionary<AppType, List<PluginEntity>> _pluginMap = new();
        private IEnumerable<PluginEntity> LoadPlugins()
        {
            var list = new List<PluginEntity>()
            {
                new PluginEntity("RestrictedArea", AppType.Revit, "SOFTAPRO"),
                new PluginEntity("ClashDefender", AppType.Navisworks, "SOFTAPRO"),
                new PluginEntity("PluginAutoCad1", AppType.AutoCAD, "Company1"),
                new PluginEntity("PluginAutoCad2", AppType.AutoCAD, "Company2"),
                new PluginEntity("PluginCivil1", AppType.Civil3D, "Company1"),
                new PluginEntity("ElementFinder", AppType.Revit, "SOFTAPRO"),
            };
            return list;
        }
        private Dictionary<AppType, List<PluginEntity>> LoadDictionaryPlugins()
        {
            var dict = new Dictionary<AppType, List<PluginEntity>>();
            foreach (var plugin in AvailableApps)
            {
                var revitPlugins = _pluginEntities.Where(x => x.App == plugin).ToList();
                dict.Add(plugin, revitPlugins);
            }
            return dict;
        }
        #endregion
    }
}