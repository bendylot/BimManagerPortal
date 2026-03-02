using BimManagerPortal.Domain.Entities.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Domain.Entities.PluginsConfigs;
using BimManagerPortal.Domain.Entities.PluginsConfigs.RestrictedAreas;
using BimManagerPortal.WebBlazorSite.Exceptions;
using BimManagerPortal.WebBlazorSite.Services.ExternalApiService;
using BimManagerPortal.WebBlazorSite.UIComponents.Layout.Modals.EventModalWindow;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BimManagerPortal.WebBlazorSite.UIComponents.PluginConfigurations.Tabs.CreateConfigurations.Forms.RestrictedAreaForm;

public partial class RestrictedAreaForm
{
    private EventModalWindow _eventModal;
    [Parameter] 
    public RestrictedAreaConfigEntity RestrictedAreaConfig { get; set; } = new();
    public RestrictedAreaConfigProxy Config { get; set; } = new();
    //public EditContext EditContext { get; set; } = default!;
    
    #region private
    private EditContext _editContext;
    [Inject]
    protected IExternalApiService ExternalApiService { get; set; }

    protected override void OnInitialized()
    {
        _editContext = new EditContext(RestrictedAreaConfig);
    }
    private List<string> ModelTypes = new()
    {
        "Умная обработка старых зон",
        "Принудительное удаление старых зон",
        "Сохранение зон, по элементам которых не построилась новая зона"
    };
    private void AddModel(PathsToModelsProxy paths)
    {
        paths.Models ??= new List<Model>();
        paths.Models.Add(new Model { ModelPath = "" });
    }

    private void RemoveModel(PathsToModelsProxy paths, int index)
    {
        if (paths.Models != null && index >= 0 && index < paths.Models.Count)
        {
            paths.Models.RemoveAt(index);
        }
    }
    protected override void OnParametersSet()
    {
        Config = RestrictedAreaConfig.Data;

        // Если вы создаете свой EditContext внутри этого компонента:
        if (_editContext == null || _editContext.Model != RestrictedAreaConfig)
        {
            _editContext = new EditContext(RestrictedAreaConfig);
        }
    }
    private async Task SendConfig()
    {
        var isValid = _editContext.Validate();

        if (!isValid)
        {
            return;
        }
        var dto = new PluginConfigRequestDto()
        {
            Name = RestrictedAreaConfig.NameConfig,
            Configuration = RestrictedAreaConfig.Data,
        };
        try
        {
            await ExternalApiService.SendPluginConfigAsync(dto);
            TestSuccess();
            // Можно добавить уведомление об успехе
        }
        catch (SendPluginConfigException ex)
        {
            // Выводим конкретно то, что мы подготовили для пользователя
            var _errorMessage = ex.UserFriendlyMessage; 
            TestError(_errorMessage);
        }
        catch (Exception ex)
        {
            // На случай непредвиденных ошибок (проблемы с сетью и т.д.)
            var _errorMessage = "Критическая ошибка приложения.";
            Console.WriteLine(ex.Message);
        }
    }
    private void TestSuccess()
    {
        _eventModal.Show(
            "Операция выполнена",
            "Данные успешно сохранены",
            true);
    }

    private void TestError(string message)
    {
        _eventModal.Show(
            "Ошибка",
            message,
            false);
    }
    #endregion
    
}