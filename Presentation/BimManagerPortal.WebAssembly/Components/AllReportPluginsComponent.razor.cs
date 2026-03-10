using System.Text.Json;
using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using BimManagerPortal.Shared.Model;
using BimManagerPortal.WebAssembly.Models.BuiltIntTab;
using BimManagerPortal.WebAssembly.Models.Results;
using BimManagerPortal.WebAssembly.Services.PluginReports;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Components;

public partial class AllReportPluginsComponent : ComponentBase
{
    #region fields
    private string? _selectedId;
    private string _searchTerm = string.Empty;
    private string? _currentSortColumn;
    private bool _sortAscending = true;
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
            (nameof(GetAllPluginBigDatasDto.Id), true) => source.OrderBy(x => x.Id),
            (nameof(GetAllPluginBigDatasDto.Id), false) => source.OrderByDescending(x => x.Id),

            (nameof(GetAllPluginBigDatasDto.PluginName), true) => source.OrderBy(x => x.PluginName),
            (nameof(GetAllPluginBigDatasDto.PluginName), false) => source.OrderByDescending(x => x.PluginName),

            _ => source
        };
    }
    private IEnumerable<GetAllPluginBigDatasDto> ApplyFiltering(IEnumerable<GetAllPluginBigDatasDto> source)
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
            return source;

        return source.Where(x =>
            x.PluginName.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
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
        try
        {
            // взять джсон элемент из апи по id
            var dto = await _pluginReportProviderServiceProvider.GetConfiguration(id);
            var jsonString = dto.Json;
            if (SelectedConfiguration.PluginName == "Запретные зоны" ||
                SelectedConfiguration.PluginName == "RestrictedArea")
            {
                // конвертируем джсон в обьект конкретного типа  BigDataBuilding
                var obj = jsonString.Deserialize<BigDataBuilding>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                // Превращаем обьект в форму отчета запретных зон
                ActiveTabChanged.InvokeAsync(new ReadPluginReportResult(obj));
            }
    }
        catch (Exception ex)
        {
            // На случай непредвиденных ошибок (проблемы с сетью и т.д.)
            var _errorMessage = "Критическая ошибка приложения.";
            Console.WriteLine(ex.Message);
        }
    }
    private async Task OpenPluginReportJson()
    {
        if (SelectedConfiguration?.Id == null) return;
        var id = SelectedConfiguration.Id;
        try
        {
            // взять джсон элемент из апи по id
            var dto = await _pluginReportProviderServiceProvider.GetConfiguration(id);
            var jsonString = dto.Json;
            // засунуть джсон в модальное окно просмотрщика
            JsonModalService.Show(jsonString);
        }
        catch (Exception ex)
        {
            // На случай непредвиденных ошибок (проблемы с сетью и т.д.)
            var _errorMessage = "Критическая ошибка приложения.";
            Console.WriteLine(ex.Message);
        }
    }
    private async Task DeletePluginReport()
    {
        if (SelectedConfiguration?.Id == null) return;
        var id = SelectedConfiguration.Id;
        try
        {
            // TODO
        }
        catch (Exception ex)
        {
            // На случай непредвиденных ошибок (проблемы с сетью и т.д.)
            var _errorMessage = "Критическая ошибка приложения.";
            Console.WriteLine(ex.Message);
        }
    }
    #endregion
}