using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BimManagerPortal.Application.Interfaces.ApiServices;
using BimManagerPortal.Application.Other.PluginsConfigs;
using BimManagerPortal.WebBlazorSite.UIComponents.Layout.Modals.EventModalWindow;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebBlazorSite.UIComponents.PluginConfigurations.Tabs.AllConfigurations
{
    public partial class AllConfiguration
    {
        #region fields
        private int? _selectedId;
        private string _searchTerm = string.Empty;
        private string? _currentSortColumn;
        private bool _sortAscending = true;
        private EventModalWindow _eventModal;
        #endregion
        
        #region properties
        private PluginConfigEntity? SelectedConfiguration => 
            Configurations?.FirstOrDefault(c => c.Id == _selectedId);
        protected IEnumerable<PluginConfigEntity>? Configurations { get; set; } = new List<PluginConfigEntity>();
        [Inject]
        protected IExternalApiService ExternalApiService { get; set; } = default!;
        private IEnumerable<PluginConfigEntity> FilteredData =>
            ApplySorting(ApplyFiltering(Configurations ?? Enumerable.Empty<PluginConfigEntity>()));
        [Parameter]
        public EventCallback<PluginConfigEntity> OnEditRequested { get; set; }
        #endregion
        
        #region events-methods
        
        protected override async Task OnInitializedAsync()
        {
            Configurations = await LoadConfigurations();
        }
        
        private void SelectRow(int? id)
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
        private IEnumerable<PluginConfigEntity> ApplyFiltering(IEnumerable<PluginConfigEntity> source)
        {
            if (string.IsNullOrWhiteSpace(_searchTerm))
                return source;

            return source.Where(x =>
                x.Name.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }
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
        #endregion
        
        #region razor methods
        private async Task RequestCreate()
        {
            await OnEditRequested.InvokeAsync(SelectedConfiguration);
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
        #endregion
        
        #region action methods

        private void EditConfiguration()
        {
            if (SelectedConfiguration == null) return;
            RequestCreate();
            // Логика перехода к редактированию
            // Например: NavigationManager.NavigateTo($"/edit/{SelectedConfiguration.Id}");
            Console.WriteLine($"Редактирование: {SelectedConfiguration.Name}");
        }

        private void OpenJsonModal()
        {
            if (SelectedConfiguration?.Data is null)
                return;

            var json = JsonSerializer.Serialize(
                SelectedConfiguration.Data,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                });

            //JsonModalService.Show(json);
        }

        private async Task DeleteConfiguration()
        {
            if (SelectedConfiguration?.Id == null) return;
            var id = SelectedConfiguration.Id.Value;
            try
            {
                await ExternalApiService.DeletePluginConfigAsync(id);
                // 1. Обновляем локальный список (удаляем элемент из памяти)
                if (Configurations != null)
                {
                    Configurations = Configurations.Where(c => c.Id != id).ToList();
                }

                // 2. Сбрасываем выделение, чтобы кнопки действий исчезли
                _selectedId = null;
                TestSuccess("Конфигурация удалена");
                // Можно добавить уведомление об успехе
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
}