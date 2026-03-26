using System.Text.Encodings.Web;
using System.Text.Json;
using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using BimManagerPortal.Shared.Model;
using BimManagerPortal.WebAssembly.Components.ModalForm.Loading;
using BimManagerPortal.WebAssembly.Models.BuiltIntTab;
using BimManagerPortal.WebAssembly.Models.Results;
using BimManagerPortal.WebAssembly.Services.PluginReports;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BimManagerPortal.WebAssembly.Components;

public partial class AllReportPluginsComponent : ComponentBase
{
    #region fields
    private string? _selectedId;
    private string _searchTerm = string.Empty;
    private string? _currentSortColumn = nameof(GetAllPluginBigDatasDto.CreatedAt);
    private bool _sortAscending = false;
    [Parameter] 
    public EventCallback<ReadPluginReportResult> ActiveTabChanged { get; set; }
    #endregion
    
    #region properties
    private GetAllPluginBigDatasDto? SelectedConfiguration => Configurations?.FirstOrDefault(c => c.Id == _selectedId);
    protected IEnumerable<GetAllPluginBigDatasDto>? Configurations { get; set; } = new List<GetAllPluginBigDatasDto>();
    private IEnumerable<GetAllPluginBigDatasDto> FilteredData => ApplySorting(ApplyFiltering(Configurations ?? Enumerable.Empty<GetAllPluginBigDatasDto>()));
    [Parameter]
    public EventCallback<GetAllPluginBigDatasDto> OnEditRequested { get; set; }
    [Inject]
    public IPluginReportProviderServiceProvider _pluginReportProviderServiceProvider { get; set; }
    [Inject]
    private LoadingModalService _loadingModalService { get; set; }
    [Inject]
    private IJSRuntime _jsRuntime { get; set; }
    #endregion
    
    #region events-methods
    
    protected override async Task OnInitializedAsync()
    {
        Configurations = await LoadConfigurations();
    }
    
    private void SelectRow(string? id)
    {
        _selectedId = id;
    }
    #endregion
    
    #region private methods

    private static string FormatJsonElement(JsonElement element)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        if (element.ValueKind == JsonValueKind.String)
        {
            using var doc = JsonDocument.Parse(element.GetString()!);
            return JsonSerializer.Serialize(doc.RootElement, options);
        }
        return JsonSerializer.Serialize(element, options);
    }
    
    private MarkupString SortIcon(string column)
    {
        if (_currentSortColumn != column)
            return new MarkupString("");

        var icon = _sortAscending ? "▲" : "▼";
        return new MarkupString($"<span class='ms-1'>{icon}</span>");
    }
    private IEnumerable<GetAllPluginBigDatasDto> ApplySorting(IEnumerable<GetAllPluginBigDatasDto> source)
    {
        if (_currentSortColumn == null)
            return source;

        return (currentSortColumn: _currentSortColumn, sortAscending: _sortAscending) switch
        {
            (nameof(GetAllPluginBigDatasDto.Id), true)  => source.OrderBy(x => x.Id),
            (nameof(GetAllPluginBigDatasDto.Id), false) => source.OrderByDescending(x => x.Id),

            (nameof(GetAllPluginBigDatasDto.PluginName), true)  => source.OrderBy(x => x.PluginName),
            (nameof(GetAllPluginBigDatasDto.PluginName), false) => source.OrderByDescending(x => x.PluginName),

            (nameof(GetAllPluginBigDatasDto.ConfigurationName), true)  => source.OrderBy(x => x.ConfigurationName),
            (nameof(GetAllPluginBigDatasDto.ConfigurationName), false) => source.OrderByDescending(x => x.ConfigurationName),

            (nameof(GetAllPluginBigDatasDto.CreatedAt), true)  => source.OrderBy(x => x.CreatedAt),
            (nameof(GetAllPluginBigDatasDto.CreatedAt), false) => source.OrderByDescending(x => x.CreatedAt),

            (nameof(GetAllPluginBigDatasDto.UserCreater), true)  => source.OrderBy(x => x.UserCreater),
            (nameof(GetAllPluginBigDatasDto.UserCreater), false) => source.OrderByDescending(x => x.UserCreater),

            _ => source
        };
    }
    private IEnumerable<GetAllPluginBigDatasDto> ApplyFiltering(IEnumerable<GetAllPluginBigDatasDto> source)
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
            return source;

        return source.Where(x =>
            x.PluginName.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
            x.ConfigurationName.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
            x.UserCreater.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
    }
    private async Task<IEnumerable<GetAllPluginBigDatasDto>> LoadConfigurations()
    {
        var list = new List<GetAllPluginBigDatasDto>();
        try
        {
            list.AddRange(await _pluginReportProviderServiceProvider.GetConfigurations());
        }
        catch (Exception ex)
        {
            // Обработка ошибки (например, вывод в консоль или UI)
            Console.WriteLine(ex.Message);
        }

        return list;
    }
    #endregion
    
    #region razor methods
    private void SortBy(string column)
    {
        if (_currentSortColumn == column)
            _sortAscending = !_sortAscending;
        else
        {
            _currentSortColumn = column;
            _sortAscending = true;
        }
    }
    #endregion
    
    #region action methods
    private async Task ReadPluginReport()
    {
        if (SelectedConfiguration?.Id == null) return;
        var id = SelectedConfiguration.Id;
        _loadingModalService.Show();
        await Task.Yield(); // дать рендереру показать модалку до старта запроса
        try
        {
            // взять джсон элемент из апи по id
            var dto = await _pluginReportProviderServiceProvider.GetConfiguration(id);
            var jsonString = dto.Json;

            // Превращаем обьект в форму отчета запретных зон
            await ActiveTabChanged.InvokeAsync(new ReadPluginReportResult(jsonString, SelectedConfiguration.PluginName));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _loadingModalService.Hide();
        }
    }
    private async Task DownloadJson()
    {
        if (SelectedConfiguration?.Id == null) return;
        var id = SelectedConfiguration.Id;
        _loadingModalService.Show();
        await Task.Yield();
        try
        {
            var dto = await _pluginReportProviderServiceProvider.GetConfiguration(id);
            var fileName = $"{SelectedConfiguration.ConfigurationName}.json";
            await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, FormatJsonElement(dto.Json));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _loadingModalService.Hide();
        }
    }
    private async Task DeletePluginReport()
    {
        if (SelectedConfiguration?.Id == null) return;
        var id = SelectedConfiguration.Id;
        _loadingModalService.Show();
        await Task.Yield();
        try
        {
            await _pluginReportProviderServiceProvider.DeleteConfiguration(id);
            _selectedId = null;
            Configurations = await LoadConfigurations();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _loadingModalService.Hide();
        }
    }
    #endregion
}