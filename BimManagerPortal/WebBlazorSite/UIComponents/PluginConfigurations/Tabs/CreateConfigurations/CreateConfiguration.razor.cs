using System.ComponentModel.DataAnnotations;
using BimManagerPortal.Domain.Entities.AppTypes;
using BimManagerPortal.Domain.Entities.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Entities.Plugins;
using BimManagerPortal.Domain.Entities.PluginsConfigs;
using BimManagerPortal.Domain.Entities.PluginsConfigs.RestrictedAreas;
using BimManagerPortal.Domain.Services.Validations;
using BimManagerPortal.WebBlazorSite.Services.ExternalApiService;
using Microsoft.AspNetCore.Components;

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
        [Parameter] public PluginConfigEntity PluginConfigEntity { get; set; } = new();
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
                PluginConfigEntity.Data = new RestrictedAreaConfigProxy();
            }
        }
        #endregion

        #region private
        [Inject]
        protected IExternalApiService ExternalApiService { get; set; }
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
        private async Task SendConfig()
        {
            var dto = new PluginConfigRequestDto()
            {
                Name = PluginConfigEntity.Name,
                Configuration = PluginConfigEntity.Data,
            };
            ValidateConfig(dto);
            try
            {
                await ExternalApiService.SendPluginConfigAsync(dto);
                // Можно добавить уведомление об успехе
            }
            catch (Exception ex)
            {
                // Обработка ошибки (например, вывод в консоль или UI)
                Console.WriteLine(ex.Message);
            }
        }

        private async Task ValidateConfig(PluginConfigRequestDto dto)
        {
            // Валидация перед отправкой
            var validationErrors = ValidationService.ValidateObjectRecursive(dto);
        
            if (validationErrors.Any())
            {
                // Показываем ошибки
                await CreatedValidationError(validationErrors);
                return;
            }
        }

        private string? _errorMessage;
        private bool _isErrorOpen;

        private async Task CreatedValidationError(List<ValidationResult> validationErrors)
        {
            var firstError = validationErrors
                .FirstOrDefault(v => !string.IsNullOrWhiteSpace(v.ErrorMessage));

            if (firstError is null)
                return;

            _errorMessage = firstError.ErrorMessage;
            _isErrorOpen = true;

            await InvokeAsync(StateHasChanged);
        }
        private void CloseError()
        {
            _isErrorOpen = false;
        }
        #endregion
    }
}