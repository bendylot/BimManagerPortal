using BimManagerPortal.Domain.Entities.PluginsConfigs;
using BimManagerPortal.WebBlazorSite.Services.ExternalApiService;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebBlazorSite.UIComponents.PluginConfigurations.Tabs.AllConfigurations
{
    public partial class AllConfiguration
    {
        #region protected
        // Вызывается автоматически при создании компонента
        protected override async Task OnInitializedAsync()
        {
            Configurations = await LoadConfigurations();
        }
        protected IEnumerable<PluginConfigEntity>? Configurations { get; set; } = new List<PluginConfigEntity>();
        [Inject]
        protected IExternalApiService ExternalApiService { get; set; } = default!;
        #endregion

        #region private
        private int? _selectedId;

        private void SelectRow(int? id)
        {
            _selectedId = id;
        }
        private string _searchTerm = string.Empty;
        private string? _currentSortColumn;
        private bool _sortAscending = true;

        private async Task<IEnumerable<PluginConfigEntity>> LoadConfigurations()
        {
            var list = new List<PluginConfigEntity>();
            try
            {
                var result = await ExternalApiService.GetPluginConfigAsync();
                var listEntity = result.Data;
                foreach (var dto in listEntity)
                {
                    if (dto.Configuration is object obj)
                    {
                        var entity = new PluginConfigEntity()
                        {
                            Name = dto.Name,
                            Id = dto.Id,
                            Data = obj,
                        };
                        list.Add(entity);
                    }
                    else
                    {
                        Console.WriteLine("dto.Configuration is not RestrictedAreaConfigProxy restrictedAreaConfigProxy");
                    }

                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки (например, вывод в консоль или UI)
                Console.WriteLine(ex.Message);
            }

            return list;
        }
        private IEnumerable<PluginConfigEntity> FilteredData =>
            ApplySorting(
                ApplyFiltering(Configurations ?? Enumerable.Empty<PluginConfigEntity>())
            );

        private IEnumerable<PluginConfigEntity> ApplyFiltering(IEnumerable<PluginConfigEntity> source)
        {
            if (string.IsNullOrWhiteSpace(_searchTerm))
                return source;

            return source.Where(x =>
                x.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<PluginConfigEntity> ApplySorting(IEnumerable<PluginConfigEntity> source)
        {
            if (_currentSortColumn == null)
                return source;

            return (currentSortColumn: _currentSortColumn, sortAscending: _sortAscending) switch
            {
                (nameof(PluginConfigEntity.Id), true) => source.OrderBy(x => x.Id),
                (nameof(PluginConfigEntity.Id), false) => source.OrderByDescending(x => x.Id),

                (nameof(PluginConfigEntity.Name), true) => source.OrderBy(x => x.Name),
                (nameof(PluginConfigEntity.Name), false) => source.OrderByDescending(x => x.Name),

                _ => source
            };
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

        private MarkupString SortIcon(string column)
        {
            if (_currentSortColumn != column)
                return new MarkupString("");

            var icon = _sortAscending ? "▲" : "▼";
            return new MarkupString($"<span class='ms-1'>{icon}</span>");
        }
        #endregion
    }
}