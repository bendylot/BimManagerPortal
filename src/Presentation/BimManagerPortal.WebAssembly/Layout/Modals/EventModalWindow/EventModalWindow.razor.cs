using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Layout.Modals.EventModalWindow;

public partial class EventModalWindow : IDisposable
{
    private bool _isVisible;
    private CancellationTokenSource _cts = new();

    [Parameter] public string Title { get; set; }
    [Parameter] public string Message { get; set; }
    [Parameter] public bool IsSuccess { get; set; }

    private string BorderClass =>
        IsSuccess ? "border-success" : "border-danger";

    private string IconClass =>
        IsSuccess ? "bi bi-check-circle-fill text-success" : "bi bi-x-circle-fill text-danger";

    public void Show(string title, string message, bool isSuccess)
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        Title = title;
        Message = message;
        IsSuccess = isSuccess;
        _isVisible = true;
        StateHasChanged();

        _ = AutoCloseAsync(_cts.Token);
    }

    private async Task AutoCloseAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(3500, token);
            _isVisible = false;
            StateHasChanged();
        }
        catch (TaskCanceledException) { }
    }

    private void Close()
    {
        _cts.Cancel();
        _isVisible = false;
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
