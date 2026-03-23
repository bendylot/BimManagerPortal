using BimManagerPortal.Application.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Application.Other.PluginsConfigs.RestrictedAreas;
using BimManagerPortal.Domain.Enums;
using BimManagerPortal.WebAssembly.Components.ModalForm.Loading;
using BimManagerPortal.WebAssembly.Layout.Modals.EventModalWindow;
using BimManagerPortal.WebAssembly.Services.PluginConfigurations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BimManagerPortal.WebAssembly.Components.PluginConfigurations.Forms.RestrictedAreaForm;

public partial class RestrictedAreaForm
{
    private EventModalWindow _eventModal = default!;
    private EditContext _editContext = default!;

    [Parameter] public PluginConfigTab PluginConfigTab { get; set; }
    [Parameter] public RestrictedAreaConfigEntity RestrictedAreaConfig { get; set; } = new();
    [Parameter] public EventCallback OnCreated { get; set; }
    [Parameter] public EventCallback OnUpdated { get; set; }

    [Inject] private IPluginConfigurationService ConfigurationService { get; set; } = default!;
    [Inject] private LoadingModalService LoadingModalService { get; set; } = default!;

    public RestrictedAreaConfigProxy Config
    {
        get => RestrictedAreaConfig.Data;
        set => RestrictedAreaConfig.Data = value;
    }

    private List<string> ModelTypes = new()
    {
        "Умная обработка старых зон",
        "Принудительное удаление старых зон",
        "Сохранение зон, по элементам которых не построилась новая зона"
    };

    protected override void OnParametersSet()
    {
        if (RestrictedAreaConfig == null)
            RestrictedAreaConfig = new RestrictedAreaConfigEntity();

        Config = RestrictedAreaConfig.Data;

        if (_editContext == null || _editContext.Model != RestrictedAreaConfig)
            _editContext = new EditContext(RestrictedAreaConfig);
    }

    private void AddModel(PathsToModelsProxy paths)
    {
        paths.Models ??= new List<Model>();
        paths.Models.Add(new Model { ModelPath = "" });
    }

    private void RemoveModel(PathsToModelsProxy paths, int index)
    {
        if (paths.Models != null && index >= 0 && index < paths.Models.Count)
            paths.Models.RemoveAt(index);
    }

    private async Task CreateConfig()
    {
        if (!_editContext.Validate()) return;

        var dto = new PluginConfigRequestDto
        {
            Name          = RestrictedAreaConfig.NameConfig,
            PluginName    = "RestrictedArea",
            Configuration = RestrictedAreaConfig.Data,
        };

        LoadingModalService.Show();
        await Task.Yield();
        try
        {
            await ConfigurationService.CreateAsync(dto);
            ShowSuccess("Конфигурация успешно создана");
            await OnCreated.InvokeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ShowError("Ошибка при создании конфигурации.");
        }
        finally
        {
            LoadingModalService.Hide();
        }
    }

    private async Task UpdateConfig()
    {
        if (!_editContext.Validate()) return;
        if (RestrictedAreaConfig.Id == null) return;

        var dto = new PluginConfigRequestDto
        {
            Name          = RestrictedAreaConfig.NameConfig,
            Configuration = RestrictedAreaConfig.Data,
        };

        LoadingModalService.Show();
        await Task.Yield();
        try
        {
            await ConfigurationService.UpdateAsync(dto, RestrictedAreaConfig.Id!);
            ShowSuccess("Конфигурация обновлена");
            await OnUpdated.InvokeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ShowError("Ошибка при обновлении конфигурации.");
        }
        finally
        {
            LoadingModalService.Hide();
        }
    }

    private void ShowSuccess(string message) =>
        _eventModal.Show("Операция выполнена", message, true);

    private void ShowError(string message) =>
        _eventModal.Show("Ошибка", message, false);
}
