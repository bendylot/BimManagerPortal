namespace BimManagerPortal.WebBlazorSite.UIComponents.UiServices.ModalForm.JsonWatcher;

public class JsonWatcherModalService
{
    public event Action<string>? OnShow;
    public event Action? OnHide;

    public void Show(string json)
    {
        OnShow?.Invoke(json);
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}