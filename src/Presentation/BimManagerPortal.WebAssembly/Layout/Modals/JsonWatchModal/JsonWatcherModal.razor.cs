using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Layout.Modals.JsonWatchModal;

public partial class JsonWatcherModal : ComponentBase
{
    private bool _showJsonModal;
    private JsonElement _jsonContent;
    protected override void OnInitialized()
    {
        JsonModalService.OnShow += ShowJsonModal;
        JsonModalService.OnHide += HideJsonModal;
    }
    public void Dispose()
    {
        JsonModalService.OnShow -= ShowJsonModal;
        JsonModalService.OnHide -= HideJsonModal;
    }
    private void ShowJsonModal(JsonElement jsonContent)
    {
        _jsonContent = jsonContent;
        _showJsonModal = true;
        InvokeAsync(StateHasChanged);
    }
    private void HideJsonModal()
    {
        _showJsonModal = false;
        InvokeAsync(StateHasChanged);
    }
    private static readonly JsonSerializerOptions _indentedOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private string FormatJson(JsonElement json)
    {
        return JsonSerializer.Serialize(json, _indentedOptions);
    }
}