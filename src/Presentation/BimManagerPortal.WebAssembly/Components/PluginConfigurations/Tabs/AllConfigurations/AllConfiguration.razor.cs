using BimManagerPortal.Application.Other.PluginsConfigs;
using BimManagerPortal.WebAssembly.Layout.Modals.EventModalWindow;
using BimManagerPortal.WebAssembly.Services.PluginConfigurations;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Components.PluginConfigurations.Tabs.AllConfigurations;

public partial class AllConfiguration
{
    private string? _selectedId;
    private string _searchTerm = string.Empty;
    private string? _currentSortColumn;
    private bool _sortAscending = true;
    private EventModalWindow _eventModal = default!;

    [Inject] private IPluginConfigurationService ConfigurationService { get; set; } = default!;

    [Parameter] public EventCallback<PluginConfigEntity> OnEditRequested { get; set; }

    private IEnumerable<PluginConfigEntity> Configurations { get; set; } = new List<PluginConfigEntity>();

    private PluginConfigEntity? SelectedConfiguration =>
        Configurations.FirstOrDefault(c => c.Id != null && c.Id == _selectedId);

    private IEnumerable<PluginConfigEntity> FilteredData =>
        ApplySorting(ApplyFiltering(Configurations));

    protected override async Task OnInitializedAsync()
    {
        Configurations = await LoadConfigurations();
    }

    private void SelectRow(string? id) => _selectedId = id;

    private MarkupString SortIcon(string column)
    {
        if (_currentSortColumn != column) return new MarkupString("");
        var icon = _sortAscending ? "▲" : "▼";
        return new MarkupString($"<span class='ms-1'>{icon}</span>");
    }

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

    private IEnumerable<PluginConfigEntity> ApplySorting(IEnumerable<PluginConfigEntity> source) =>
        (_currentSortColumn, _sortAscending) switch
        {
            (nameof(PluginConfigEntity.Name), true)       => source.OrderBy(x => x.Name),
            (nameof(PluginConfigEntity.Name), false)      => source.OrderByDescending(x => x.Name),
            (nameof(PluginConfigEntity.PluginName), true)  => source.OrderBy(x => x.PluginName),
            (nameof(PluginConfigEntity.PluginName), false) => source.OrderByDescending(x => x.PluginName),
            (nameof(PluginConfigEntity.CreatedAt), true)  => source.OrderBy(x => x.CreatedAt),
            (nameof(PluginConfigEntity.CreatedAt), false) => source.OrderByDescending(x => x.CreatedAt),
            _ => source
        };

    private IEnumerable<PluginConfigEntity> ApplyFiltering(IEnumerable<PluginConfigEntity> source)
    {
        if (string.IsNullOrWhiteSpace(_searchTerm)) return source;
        return source.Where(x =>
            x.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
            (x.PluginName?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ?? false));
    }

    private async Task<IEnumerable<PluginConfigEntity>> LoadConfigurations()
    {
        try
        {
            return await ConfigurationService.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<PluginConfigEntity>();
        }
    }

    private void EditConfiguration()
    {
        if (SelectedConfiguration == null) return;
        OnEditRequested.InvokeAsync(SelectedConfiguration);
    }

    private async Task DeleteConfiguration()
    {
        if (SelectedConfiguration?.Id == null) return;
        var id = SelectedConfiguration.Id;
        try
        {
            await ConfigurationService.DeleteAsync(id);
            Configurations = Configurations.Where(c => c.Id != id).ToList();
            _selectedId = null;
            _eventModal.Show("Операция выполнена", "Конфигурация удалена", true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            _eventModal.Show("Ошибка", "Не удалось удалить конфигурацию.", false);
        }
    }
}
