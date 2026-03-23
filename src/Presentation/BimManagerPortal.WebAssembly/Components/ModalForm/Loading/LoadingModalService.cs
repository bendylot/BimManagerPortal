namespace BimManagerPortal.WebAssembly.Components.ModalForm.Loading;

public class LoadingModalService
{
    public event Action? OnShow;
    public event Action? OnHide;

    public void Show()
    {
        OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}
