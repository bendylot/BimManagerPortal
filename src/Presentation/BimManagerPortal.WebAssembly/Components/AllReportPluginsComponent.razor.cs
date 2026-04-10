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

    private PagedResultDto<GetAllPluginBigDatasDto> _pagedResult = new();
    private int _currentPage = 1;
    private const int PageSize = 20;

    private CancellationTokenSource? _searchCts;

    [Parameter]
    public EventCallback<ReadPluginReportResult> ActiveTabChanged { get; set; }
    #endregion

    #region properties
    private GetAllPluginBigDatasDto? SelectedConfiguration =>
        _pagedResult.Items.FirstOrDefault(c => c.Id == _selectedId);

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
        await LoadPage();
    }

    private void SelectRow(string? id)
    {
        _selectedId = id;
    }

    private async Task OnSearchInput(ChangeEventArgs e)
    {
        _searchTerm = e.Value?.ToString() ?? "";
        _currentPage = 1;
        _selectedId = null;

        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        var token = _searchCts.Token;

        try
        {
            await Task.Delay(300, token);
            await LoadPage();
        }
        catch (OperationCanceledException) { }
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

    private async Task LoadPage()
    {
        try
        {
            _pagedResult = await _pluginReportProviderServiceProvider.GetConfigurationsPaged(
                _currentPage, PageSize, _searchTerm, _currentSortColumn, _sortAscending);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        StateHasChanged();
    }
    #endregion

    #region razor methods
    private async Task SortBy(string column)
    {
        if (_currentSortColumn == column)
            _sortAscending = !_sortAscending;
        else
        {
            _currentSortColumn = column;
            _sortAscending = true;
        }
        _currentPage = 1;
        _selectedId = null;
        await LoadPage();
    }

    private async Task GoToPage(int page)
    {
        if (page < 1 || page > _pagedResult.TotalPages) return;
        _currentPage = page;
        _selectedId = null;
        await LoadPage();
    }

    // Returns page numbers with -1 as ellipsis placeholder
    private IEnumerable<int> GetPageNumbers()
    {
        var total = _pagedResult.TotalPages;
        var cur = _currentPage;
        var pages = new List<int>();

        if (total <= 7)
        {
            for (var i = 1; i <= total; i++) pages.Add(i);
            return pages;
        }

        pages.Add(1);
        if (cur > 3) pages.Add(-1);
        for (var i = Math.Max(2, cur - 1); i <= Math.Min(total - 1, cur + 1); i++)
            pages.Add(i);
        if (cur < total - 2) pages.Add(-1);
        pages.Add(total);

        return pages;
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
            await ActiveTabChanged.InvokeAsync(new ReadPluginReportResult(id, jsonString, SelectedConfiguration.PluginName));
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
            // если удалили последний элемент на странице — переходим на предыдущую
            if (_pagedResult.Items.Count == 1 && _currentPage > 1)
                _currentPage--;
            await LoadPage();
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