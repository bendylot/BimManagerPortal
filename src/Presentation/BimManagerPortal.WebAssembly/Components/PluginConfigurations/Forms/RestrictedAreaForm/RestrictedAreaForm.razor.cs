using BimManagerPortal.Domain.Enums;
using BimManagerPortal.Shared.Other.Dtos.Requests.PluginConfigs;
using BimManagerPortal.Shared.Other.PluginsConfigs.RestrictedAreas;
using BimManagerPortal.WebAssembly.Components.ModalForm.Loading;
using BimManagerPortal.WebAssembly.Layout.Modals.EventModalWindow;
using BimManagerPortal.WebAssembly.Services.PluginConfigurations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

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
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

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
        paths.Models ??= new List<ModelPath>();
        paths.Models.Add(new ModelPath());
    }

    private void RemoveModel(PathsToModelsProxy paths, int index)
    {
        if (paths.Models != null && index >= 0 && index < paths.Models.Count)
            paths.Models.RemoveAt(index);
    }

    // ── Import tab state ──────────────────────────────────────────
    private int _activeImportTab = 0;

    // Tab 0: folder import
    private string _folderPath = "";
    private List<string> _folderFileNames = new();

    // Tab 1: paste import
    private string _pasteText = "";

    private int ParsedPasteCount
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_pasteText)) return 0;
            var matches = System.Text.RegularExpressions.Regex.Matches(_pasteText, "\"([^\"]+)\"");
            if (matches.Count > 0) return matches.Count;
            return _pasteText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                             .Count(l => !string.IsNullOrWhiteSpace(l.Trim()));
        }
    }

    private async Task OpenFolderFilePicker()
    {
        await JSRuntime.InvokeVoidAsync("eval", "document.getElementById('rvtFolderFilePicker').click()");
    }

    private void HandleFolderFilesSelected(InputFileChangeEventArgs e)
    {
        _folderFileNames = e.GetMultipleFiles(200).Select(f => f.Name).ToList();
    }

    private void ApplyFolderImport()
    {
        if (string.IsNullOrWhiteSpace(_folderPath) || _folderFileNames.Count == 0) return;

        var folder = _folderPath.TrimEnd('\\');
        foreach (var name in _folderFileNames)
            Config.PathsToModels.Models.Add(new ModelPath { Model = $"{folder}\\{name}" });

        _folderFileNames = new();
        _folderPath = "";
    }

    private void ApplyPasteImport()
    {
        if (string.IsNullOrWhiteSpace(_pasteText)) return;

        var matches = System.Text.RegularExpressions.Regex.Matches(_pasteText, "\"([^\"]+)\"");
        if (matches.Count > 0)
        {
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                var path = m.Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(path))
                    Config.PathsToModels.Models.Add(new ModelPath { Model = path });
            }
        }
        else
        {
            foreach (var line in _pasteText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var path = line.Trim().Trim('"');
                if (!string.IsNullOrEmpty(path))
                    Config.PathsToModels.Models.Add(new ModelPath { Model = path });
            }
        }

        _pasteText = "";
    }

    private void ClearAllModels() => Config.PathsToModels.Models.Clear();

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
                //ShowSuccess("Конфигурация обновлена");
            await OnUpdated.InvokeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //ShowError("Ошибка при обновлении конфигурации.");
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
