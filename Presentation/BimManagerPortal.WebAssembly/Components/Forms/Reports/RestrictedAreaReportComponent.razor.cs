using BimManagerPortal.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Components.Forms.Reports;

public partial class RestrictedAreaReportComponent : ComponentBase
{
    [Parameter]
    public RestrictedAreaReportModel RestrictedAreaReportModel { get; set; }
    private string FormatDate(DateTime? date)
    {
        return date?.ToString("dd.MM.yyyy HH:mm:ss") ?? "—";
    }

    private string FormatTimeSpan(TimeSpan? timeSpan)
    {
        if (!timeSpan.HasValue) return "—";
        return $"{(int)timeSpan.Value.TotalHours:D2}:{timeSpan.Value.Minutes:D2}:{timeSpan.Value.Seconds:D2}";
    }
}