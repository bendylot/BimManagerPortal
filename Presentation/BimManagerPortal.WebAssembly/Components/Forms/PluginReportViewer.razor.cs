using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Components.Forms;

public partial class PluginReportViewer : ComponentBase
{
    [Parameter] public object? Value { get; set; }
    [Parameter] public string Name { get; set; } = "";
    [Parameter] public int Level { get; set; }

    bool expanded = false;

    void Toggle()
    {
        expanded = !expanded;
    }

    bool IsComplex =>
        Value != null &&
        !(Value is string) &&
        !(Value.GetType().IsPrimitive) &&
        !(Value is DateTime) &&
        !(Value is TimeSpan);

    IEnumerable<(string Name, object? Value)> GetChildren()
    {
        if (Value == null)
            yield break;

        if (Value is IEnumerable enumerable && Value is not string)
        {
            int i = 0;

            foreach (var item in enumerable)
            {
                yield return ($"[{i}]", item);
                i++;
            }

            yield break;
        }

        var props = Value.GetType().GetProperties(
            BindingFlags.Public | BindingFlags.Instance);

        foreach (var p in props)
        {
            yield return (p.Name, p.GetValue(Value));
        }
    }

    string FormatValue(object? value)
    {
        if (value == null)
            return "null";

        if (value is DateTime dt)
            return dt.ToString("yyyy-MM-dd HH:mm:ss");

        if (value is TimeSpan ts)
            return ts.ToString();

        return value.ToString() ?? "";
    }
}