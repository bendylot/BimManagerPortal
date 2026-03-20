using System.Text.Json;

namespace BimManagerPortal.WebAssembly.Components.ModalForm.JsonWatcher;

public class JsonWatcherModalService
{
    public event Action<JsonElement>? OnShow;
    public event Action? OnHide;

    public void Show(JsonElement json)
    {
        OnShow?.Invoke(json);
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}