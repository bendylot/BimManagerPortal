using BimManagerPortal.Application.Interfaces.ApiServices;
using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Application.Other.Exceptions;
using BimManagerPortal.Application.Other.PluginsConfigs.RestrictedAreas;
using BimManagerPortal.Domain.Enums;
using BimManagerPortal.WebBlazorSite.UIComponents.Layout.Modals.EventModalWindow;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BimManagerPortal.WebBlazorSite.UIComponents.PluginConfigurations.PluginConfigurationForms.RestrictedAreaForm;

public partial class RestrictedAreaForm
{
    private EventModalWindow _eventModal;
    private RestrictedAreaConfigEntity _model = new ();
    [Parameter] 
    public PluginConfigTab PluginConfigTab { get; set; }
    [Parameter] 
    public RestrictedAreaConfigEntity RestrictedAreaConfig { get; set; }
    public RestrictedAreaConfigProxy Config {
        get => RestrictedAreaConfig.Data;
        set => RestrictedAreaConfig.Data = value;
}
    //public EditContext EditContext { get; set; } = default!;
    
    #region private
    private EditContext _editContext;
    [Inject]
    protected IExternalApiService ExternalApiService { get; set; }

    /*protected override void OnInitialized()
    {
        _editContext = new EditContext(RestrictedAreaConfig);
    }*/
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
        if (RestrictedAreaConfig == null)
        {
            RestrictedAreaConfig = new RestrictedAreaConfigEntity();
        }
        Config = RestrictedAreaConfig.Data;

        // Если вы создаете свой EditContext внутри этого компонента:
        if (_editContext == null || _editContext.Model != RestrictedAreaConfig)
        {
            _editContext = new EditContext(RestrictedAreaConfig);
        }
        
    }
    private async Task CreateConfig()
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
            TestSuccess("Конфигурация создана");
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
    private async Task UpdateConfig()
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
            await ExternalApiService.UpdateExistPluginConfigAsync(dto, RestrictedAreaConfig.Id.Value);
            TestSuccess("Конфигурация обновлена");
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
    private void TestSuccess(string message)
    {
        _eventModal.Show(
            "Операция выполнена",
            message,
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