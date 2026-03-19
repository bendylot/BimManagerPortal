namespace BimManagerPortal.Shared.Model;

// ==================== ROOT ====================
public class RestrictedAreaReportModel
{
    public RestrictedAreaReportModel(
        CommonInformationBuilding commonInformationBuilding,
        List<ObjectConiguratorData> userConfiguratorData)
    {
        this.CommonInformationBuilding = commonInformationBuilding;
        this.ObjectConiguratorData = userConfiguratorData;
    }
    public CommonInformationBuilding CommonInformationBuilding { get; set; }
    public List<ObjectConiguratorData> ObjectConiguratorData { get; set; }
}

// ==================== MEASURING TIME ====================
public abstract class MeasuringTime
{
    public MeasuringTime(DateTime dateStartProcess, DateTime dateEndProcess)
    {
        this.DateStartProcess = dateStartProcess;
        this.DateEndProcess = dateEndProcess;
    }
    public DateTime DateStartProcess { get; set; }
    public DateTime DateEndProcess { get; set; }
    public TimeSpan ComputeProcessTime => DateEndProcess - DateStartProcess;
}

// ==================== COMMON INFO ====================
public class CommonInformationBuilding : MeasuringTime
{
    public CommonInformationBuilding(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string userName,
        string pluginVersion) : base(dateStartProcess, dateEndProcess)
    {
        this.UserName = userName;
        this.PluginVersion = pluginVersion;
    }
    public string UserName { get; set; }
    public string PluginVersion { get; set; }
}

// ==================== OBJECT CONFIGURATOR ====================
public class ObjectConiguratorData : MeasuringTime
{
    public ObjectConiguratorData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string objectName,
        List<SectionBuildingData> arBuildingData,
        List<DocumentNotHandledError> errorDocumentHandler) : base(dateStartProcess, dateEndProcess)
    {
        this.ObjectName = objectName;
        this.SectionsBuildingData = arBuildingData;
        this.ErrorDocumentHandler = errorDocumentHandler;
    }
    public string ObjectName { get; set; }
    public List<SectionBuildingData> SectionsBuildingData { get; set; }
    public List<DocumentNotHandledError> ErrorDocumentHandler { get; private set; }
}

// ==================== SECTION ====================
public class SectionBuildingData : MeasuringTime
{
    public SectionBuildingData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string sectionBuildingDataName,
        List<DocumentBuildingData> documentsBuildingData) : base(dateStartProcess, dateEndProcess)
    {
        this.SectionBuildingDataName = sectionBuildingDataName;
        this.DocumentsBuildingData = documentsBuildingData;
    }
    public string SectionBuildingDataName { get; set; }
    public List<DocumentBuildingData> DocumentsBuildingData { get; set; }
}

// ==================== DOCUMENT ====================
public class DocumentBuildingData : MeasuringTime
{
    public DocumentBuildingData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string documentTitle,
        string documentSection,
        List<EntityBuildingData> entityBuildingData,
        DocumentDeletingZonesResult documentDeletingZonesResult) : base(dateStartProcess, dateEndProcess)
    {
        this.DocumentTitle = documentTitle;
        this.DocumentSection = documentSection;
        this.EntityBuildingData = entityBuildingData;
        this.DocumentDeletingZonesResult = documentDeletingZonesResult;
    }
    public string DocumentTitle { get; set; }
    public string DocumentSection { get; set; }
    public List<EntityBuildingData> EntityBuildingData { get; set; }
    public DocumentDeletingZonesResult DocumentDeletingZonesResult { get; set; }
}

// ==================== ENTITY ====================
public class EntityBuildingData : MeasuringTime
{
    public EntityBuildingData(
        DateTime dateStartProcess,
        DateTime dateEndProcess,
        string entityName,
        List<ElementEntity> hostElements,
        List<ElementEntity> createdElements,
        NotCreatedElementsData notCreatedElementsData,
        DeletingZonesEntityResult deletingZonesEntityResult) : base(dateStartProcess, dateEndProcess)
    {
        this.EntityName = entityName;
        this.HostElements = hostElements;
        this.CreatedElements = createdElements;
        this.NotCreatedElementsData = notCreatedElementsData;
        this.DeletingZonesEntityResult = deletingZonesEntityResult;
    }
    public string EntityName { get; set; }
    public List<ElementEntity> HostElements { get; set; }
    public List<ElementEntity> CreatedElements { get; set; }
    public NotCreatedElementsData NotCreatedElementsData { get; set; }
    public DeletingZonesEntityResult DeletingZonesEntityResult { get; set; }
}

// ==================== NOT CREATED ====================
public class NotCreatedElementsData
{
    public NotCreatedElementsData(
        List<NotCreatedElementError> goodNotCreatedElements,
        List<NotCreatedElementError> badNotCreatedElements)
    {
        this.GoodNotCreatedElements = goodNotCreatedElements;
        this.BadNotCreatedElements = badNotCreatedElements;
    }
    public List<NotCreatedElementError> GoodNotCreatedElements { get; set; }
    public List<NotCreatedElementError> BadNotCreatedElements { get; set; }
}

// ==================== ERRORS ====================
public abstract class ElementError
{
    public ElementError(string reasonNotCreated)
    {
        this.ReasonNotCreated = reasonNotCreated;
    }
    public string ReasonNotCreated { get; set; }
}

public class NotCreatedElementError : ElementError
{
    public NotCreatedElementError(
        string badElementId,
        string reasonNotCreated) : base(badElementId)
    {
        this.BadElementId = badElementId;
        this.ReasonNotCreated = reasonNotCreated;
    }
    public string BadElementId { get; set; }
    public string ReasonNotCreated { get; set; }
}

public class DocumentNotHandledError : ElementError
{
    public DocumentNotHandledError(
        string reasonNotCreated,
        string modelPath) : base(reasonNotCreated)
    {
        this.ReasonNotCreated = reasonNotCreated;
        this.ModelPath = modelPath;
    }
    public string ReasonNotCreated { get; set; }
    public string ModelPath { get; set; }
}

// ==================== DELETING ZONES ====================
public class DeletingZonesEntityResult
{
    public DeletingZonesEntityResult(List<ElementEntity> savedOldZones)
    {
        this.SavedOldZones = savedOldZones;
    }
    public List<ElementEntity> SavedOldZones { get; set; }
}

public class DocumentDeletingZonesResult
{
    public DocumentDeletingZonesResult(
        List<string> deletedOldZones,
        List<ElementEntity> notDeletedBusyOldZones)
    {
        this.DeletedOldZones = deletedOldZones;
        this.NotDeletedBusyOldZones = notDeletedBusyOldZones;
    }
    public List<string> DeletedOldZones { get; set; }
    public List<ElementEntity> NotDeletedBusyOldZones { get; set; }
}

public class ElementEntity
{
    public ElementEntity(string elementId)
    {
        this.ElementId = elementId;
    }
    public string ElementId { get; private set; }
}