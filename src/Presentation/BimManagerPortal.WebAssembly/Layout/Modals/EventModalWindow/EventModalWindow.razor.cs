using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Layout.Modals.EventModalWindow;
public partial class EventModalWindow
{
    private bool _isVisible;

    [Parameter] public string Title { get; set; }
    [Parameter] public string Message { get; set; }
    [Parameter] public bool IsSuccess { get; set; }

    private string BorderClass =>
        IsSuccess ? "border border-3 border-success"
            : "border border-3 border-danger";

    public void Show(string title, string message, bool isSuccess)
    {
        Title = title;
        Message = message;
        IsSuccess = isSuccess;
        _isVisible = true;
        StateHasChanged();
    }

    private void Close()
    {
        _isVisible = false;
    }
}