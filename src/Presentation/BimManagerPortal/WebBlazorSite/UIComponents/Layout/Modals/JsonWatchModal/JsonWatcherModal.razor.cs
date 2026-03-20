using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebBlazorSite.UIComponents.Layout.Modals.JsonWatchModal;

public partial class JsonWatcherModal : ComponentBase
{
    private bool _showJsonModal;
    private string _jsonContent = string.Empty;
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
    private void ShowJsonModal(string json)
    {
        _jsonContent = json;
        _showJsonModal = true;
        InvokeAsync(StateHasChanged);
    }
    private void HideJsonModal()
    {
        _showJsonModal = false;
        InvokeAsync(StateHasChanged);
    }
}