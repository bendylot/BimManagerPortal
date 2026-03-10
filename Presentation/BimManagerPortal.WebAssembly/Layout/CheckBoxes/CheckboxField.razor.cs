using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Layout.CheckBoxes;

public partial class CheckboxField
{
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public bool Value { get; set; }
    [Parameter] public EventCallback<bool> ValueChanged { get; set; }
    [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString();
}