using BimManagerPortal.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebAssembly.Components.Forms.Reports;

public partial class RestrictedAreaReportComponent : ComponentBase
{
    [Parameter]
    public RestrictedAreaReportModel RestrictedAreaReportModel { get; set; }

    private readonly HashSet<int> _expandedObjects         = new();
    private readonly HashSet<int> _expandedSections        = new();
    private readonly HashSet<int> _expandedDocuments       = new();
    private readonly HashSet<int> _expandedBadErrors       = new();
    private readonly HashSet<int> _expandedGoodErrors      = new();
    private readonly HashSet<int> _expandedHostElements    = new();
    private readonly HashSet<int> _expandedCreatedElements = new();
    private readonly HashSet<int> _expandedSavedZones      = new();
    private readonly HashSet<int> _expandedDeletedZones    = new();
    private readonly HashSet<int> _expandedNotDeletedZones = new();
    private readonly HashSet<int> _expandedErrorDocs       = new();

    private bool _allExpanded = false;

    private string FormatDate(DateTime? date) => date?.ToString("dd.MM.yyyy HH:mm:ss") ?? "—";

    private string FormatTimeSpan(TimeSpan? timeSpan)
    {
        if (!timeSpan.HasValue) return "—";
        return $"{(int)timeSpan.Value.TotalHours:D2}:{timeSpan.Value.Minutes:D2}:{timeSpan.Value.Seconds:D2}";
    }

    private void ToggleSet(HashSet<int> set, int key)
    {
        if (!set.Add(key)) set.Remove(key);
    }

    private void ToggleExpandAll()
    {
        _allExpanded = !_allExpanded;
        _expandedObjects.Clear();
        if (_allExpanded && RestrictedAreaReportModel.ObjectConiguratorData != null)
        {
            for (var i = 0; i < RestrictedAreaReportModel.ObjectConiguratorData.Count; i++)
                _expandedObjects.Add(i);
        }
    }

    private record SummaryStats(
        int TotalObjects, int TotalDocuments, int TotalEntities,
        int TotalCreated, int TotalGoodNotCreated, int TotalBadNotCreated,
        int TotalDeletedZones, int TotalBusyZones);

    private SummaryStats ComputeSummary()
    {
        var objs = RestrictedAreaReportModel.ObjectConiguratorData;
        if (objs == null) return new(0, 0, 0, 0, 0, 0, 0, 0);

        int docs = 0, entities = 0, created = 0, good = 0, bad = 0, deleted = 0, busy = 0;
        foreach (var obj in objs)
        {
            foreach (var sec in obj.SectionsBuildingData ?? [])
            {
                docs += sec.DocumentsBuildingData?.Count ?? 0;
                foreach (var doc in sec.DocumentsBuildingData ?? [])
                {
                    entities += doc.EntityBuildingData?.Count ?? 0;
                    foreach (var ent in doc.EntityBuildingData ?? [])
                    {
                        created += ent.CreatedElements?.Count ?? 0;
                        good    += ent.NotCreatedElementsData?.GoodNotCreatedElements?.Count ?? 0;
                        bad     += ent.NotCreatedElementsData?.BadNotCreatedElements?.Count ?? 0;
                    }
                    deleted += doc.DocumentDeletingZonesResult?.DeletedOldZones?.Count ?? 0;
                    busy    += doc.DocumentDeletingZonesResult?.NotDeletedBusyOldZones?.Count ?? 0;
                }
            }
        }
        return new(objs.Count, docs, entities, created, good, bad, deleted, busy);
    }

    private record ObjectStats(int Created, int BadErrors, int DocErrors);

    private ObjectStats ComputeObjectStats(ObjectConiguratorData obj)
    {
        int created = 0, bad = 0;
        int docErr = obj.ErrorDocumentHandler?.Count ?? 0;
        foreach (var sec in obj.SectionsBuildingData ?? [])
        foreach (var doc in sec.DocumentsBuildingData ?? [])
        foreach (var ent in doc.EntityBuildingData ?? [])
        {
            created += ent.CreatedElements?.Count ?? 0;
            bad     += ent.NotCreatedElementsData?.BadNotCreatedElements?.Count ?? 0;
        }
        return new(created, bad, docErr);
    }

    private record DocStats(int Created, int Bad);

    private DocStats ComputeDocStats(DocumentBuildingData doc)
    {
        int created = 0, bad = 0;
        foreach (var ent in doc.EntityBuildingData ?? [])
        {
            created += ent.CreatedElements?.Count ?? 0;
            bad     += ent.NotCreatedElementsData?.BadNotCreatedElements?.Count ?? 0;
        }
        return new(created, bad);
    }
}
