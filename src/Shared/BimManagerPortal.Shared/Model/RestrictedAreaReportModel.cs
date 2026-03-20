namespace BimManagerPortal.Shared.Model;

// ==================== ROOT ====================
public class RestrictedAreaReportModel
{
    public RestrictedAreaReportModel() { }

    public RestrictedAreaReportModel(
        CommonInformationBuilding commonInformationBuilding,
        List<ObjectConiguratorData> objectConiguratorData) // ← имя = свойство
    {
        CommonInformationBuilding = commonInformationBuilding;
        ObjectConiguratorData = objectConiguratorData;
    }

    public CommonInformationBuilding CommonInformationBuilding { get; set; }
    public List<ObjectConiguratorData> ObjectConiguratorData { get; set; }
}

public abstract class MeasuringTime
{
    protected MeasuringTime() { } // ← добавить пустой конструктор

    public MeasuringTime(DateTime dateStartProcess, DateTime dateEndProcess)
    {
        DateStartProcess = dateStartProcess;
        DateEndProcess = dateEndProcess;
    }

    public DateTime DateStartProcess { get; set; }
    public DateTime DateEndProcess { get; set; }
    public TimeSpan ComputeProcessTime => DateEndProcess - DateStartProcess;
}

public class CommonInformationBuilding : MeasuringTime
{
    public CommonInformationBuilding() { }

    public CommonInformationBuilding(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string userName,
        string pluginVersion) : base(dateStartProcess, dateEndProcess)
    {
        UserName = userName;
        PluginVersion = pluginVersion;
    }

    public string UserName { get; set; }
    public string PluginVersion { get; set; }
}

public class ObjectConiguratorData : MeasuringTime
{
    public ObjectConiguratorData() { }

    public ObjectConiguratorData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string objectName,
        List<SectionBuildingData> sectionsBuildingData, // ← имя = свойство
        List<DocumentNotHandledError> errorDocumentHandler) : base(dateStartProcess, dateEndProcess)
    {
        ObjectName = objectName;
        SectionsBuildingData = sectionsBuildingData;
        ErrorDocumentHandler = errorDocumentHandler;
    }

    public string ObjectName { get; set; }
    public List<SectionBuildingData> SectionsBuildingData { get; set; }
    public List<DocumentNotHandledError> ErrorDocumentHandler { get; set; } // ← убрать private
}

public class SectionBuildingData : MeasuringTime
{
    public SectionBuildingData() { }

    public SectionBuildingData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string sectionBuildingDataName,
        List<DocumentBuildingData> documentsBuildingData) : base(dateStartProcess, dateEndProcess)
    {
        SectionBuildingDataName = sectionBuildingDataName;
        DocumentsBuildingData = documentsBuildingData;
    }

    public string SectionBuildingDataName { get; set; }
    public List<DocumentBuildingData> DocumentsBuildingData { get; set; }
}

public class DocumentBuildingData : MeasuringTime
{
    public DocumentBuildingData() { }

    public DocumentBuildingData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string documentTitle,
        string documentSection,
        List<EntityBuildingData> entityBuildingData,
        DocumentDeletingZonesResult documentDeletingZonesResult) : base(dateStartProcess, dateEndProcess)
    {
        DocumentTitle = documentTitle;
        DocumentSection = documentSection;
        EntityBuildingData = entityBuildingData;
        DocumentDeletingZonesResult = documentDeletingZonesResult;
    }

    public string DocumentTitle { get; set; }
    public string DocumentSection { get; set; }
    public List<EntityBuildingData> EntityBuildingData { get; set; }
    public DocumentDeletingZonesResult DocumentDeletingZonesResult { get; set; }
}

public class EntityBuildingData : MeasuringTime
{
    public EntityBuildingData() { }

    public EntityBuildingData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string entityName,
        List<ElementEntity> hostElements,
        List<ElementEntity> createdElements,
        NotCreatedElementsData notCreatedElementsData,
        List<ElementEntity> savedOldZones) : base(dateStartProcess, dateEndProcess)
    {
        EntityName = entityName;
        HostElements = hostElements;
        CreatedElements = createdElements;
        NotCreatedElementsData = notCreatedElementsData;
        SavedOldZones = savedOldZones;
    }

    public string EntityName { get; set; }
    public List<ElementEntity> HostElements { get; set; }
    public List<ElementEntity> CreatedElements { get; set; }
    public NotCreatedElementsData NotCreatedElementsData { get; set; }
    public List<ElementEntity> SavedOldZones { get; set; }
}

public class NotCreatedElementsData
{
    public NotCreatedElementsData() { }

    public NotCreatedElementsData(
        List<NotCreatedElementError> goodNotCreatedElements,
        List<NotCreatedElementError> badNotCreatedElements)
    {
        GoodNotCreatedElements = goodNotCreatedElements;
        BadNotCreatedElements = badNotCreatedElements;
    }

    public List<NotCreatedElementError> GoodNotCreatedElements { get; set; }
    public List<NotCreatedElementError> BadNotCreatedElements { get; set; }
}

public abstract class ElementError
{
    protected ElementError() { }

    public ElementError(string reasonNotCreated)
    {
        ReasonNotCreated = reasonNotCreated;
    }

    public string ReasonNotCreated { get; set; }
}

public class NotCreatedElementError : ElementError
{
    public NotCreatedElementError() { }

    public NotCreatedElementError(
        string badElementId,
        string reasonNotCreated) : base(reasonNotCreated)
    {
        BadElementId = badElementId;
        ReasonNotCreated = reasonNotCreated;
    }

    public string BadElementId { get; set; }
    public string ReasonNotCreated { get; set; }
}

public class DocumentNotHandledError : ElementError
{
    public DocumentNotHandledError() { }

    public DocumentNotHandledError(
        string reasonNotCreated,
        string modelPath) : base(reasonNotCreated)
    {
        ReasonNotCreated = reasonNotCreated;
        ModelPath = modelPath;
    }

    public string ReasonNotCreated { get; set; }
    public string ModelPath { get; set; }
}

public class DocumentDeletingZonesResult
{
    public DocumentDeletingZonesResult() { }

    public DocumentDeletingZonesResult(
        List<string> deletedOldZones,
        List<ElementEntity> notDeletedBusyOldZones)
    {
        DeletedOldZones = deletedOldZones;
        NotDeletedBusyOldZones = notDeletedBusyOldZones;
    }

    public List<string> DeletedOldZones { get; set; }
    public List<ElementEntity> NotDeletedBusyOldZones { get; set; }
}

public class ElementEntity
{
    public ElementEntity() { }

    public ElementEntity(string elementId)
    {
        ElementId = elementId;
    }

    public string ElementId { get; set; } // ← убрать private
}