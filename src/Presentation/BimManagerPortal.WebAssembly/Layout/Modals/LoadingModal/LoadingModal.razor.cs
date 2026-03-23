using BimManagerPortal.WebAssembly.Components.ModalForm.Loading;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Layout.Modals.LoadingModal;

public partial class LoadingModal : ComponentBase, IDisposable
{
    [Inject] private LoadingModalService LoadingModalService { get; set; } = default!;

    private bool _isVisible;

    protected override void OnInitialized()
    {
        LoadingModalService.OnShow += Show;
        LoadingModalService.OnHide += Hide;
    }

    private void Show()
    {
        _isVisible = true;
        InvokeAsync(StateHasChanged);
    }

    private void Hide()
    {
        _isVisible = false;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        LoadingModalService.OnShow -= Show;
        LoadingModalService.OnHide -= Hide;
    }
}
