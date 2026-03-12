namespace BimManagerPortal.Shared.Model;

public class RestrictedAreaReportModel
{
    public RestrictedAreaReportModel() { }
    public RestrictedAreaReportModel(CommonInformationBuilding CommonInformationBuilding, List<ObjectConiguratorData>  UserConfiguratorData)
    {
        this.CommonInformationBuilding = CommonInformationBuilding;
        this.ObjectConiguratorData = UserConfiguratorData;
    }
    public CommonInformationBuilding CommonInformationBuilding { get; set; }
    public List<ObjectConiguratorData>  ObjectConiguratorData { get; set; }
    
}
public abstract class MeasuringTime
{
    public MeasuringTime() { }
    public MeasuringTime(DateTime DateStartProcess, DateTime DateEndProcess)
    {
        this.DateStartProcess = DateStartProcess;
        this.DateEndProcess = DateEndProcess;
    }
    public DateTime DateStartProcess { get; set; }
    public DateTime DateEndProcess { get; set; }
    public TimeSpan ComputeProcessTime => DateEndProcess - DateStartProcess;
}
public class CommonInformationBuilding : MeasuringTime
{
    public CommonInformationBuilding() { }
    public CommonInformationBuilding(DateTime DateStartProcess, DateTime DateEndProcess, string userName,string PluginVersion) : base(DateStartProcess, DateEndProcess)
    {
        this.UserName = userName;
        this.PluginVersion = PluginVersion;
    }
    public string UserName { get; set; }
    public string PluginVersion { get; set; }
}
public class ObjectConiguratorData : MeasuringTime
{
    public ObjectConiguratorData() { }
    public ObjectConiguratorData(DateTime DateStartProcess, 
        DateTime DateEndProcess, 
        string ObjectName,
        List<SectionBuildingData> ArBuildingData,
        List<DocumentNotHandledError> ErrorDocumentHandler) : base(DateStartProcess, DateEndProcess)
    {
        this.ObjectName = ObjectName;
        //this.ObjectConiguratorName = ObjectConiguratorName;
        this.SectionsBuildingData = ArBuildingData;
        this.ErrorDocumentHandler = ErrorDocumentHandler;
    }
    //public string ObjectConiguratorName { get; set; }
    public string ObjectName { get; set; }
    public List<SectionBuildingData> SectionsBuildingData { get; set; }
    public List<DocumentNotHandledError> ErrorDocumentHandler { get; private set; }
    //public int NumberErrorsSectionObjectConigurator => SectionsBuildingData.Sum(x=>x.NumberErrorsDocumentsSection);
}

public class SectionBuildingData : MeasuringTime
{
    public SectionBuildingData() { }
    public SectionBuildingData(DateTime DateStartProcess, 
        DateTime DateEndProcess, 
        string SectionBuildingDataName, 
        List<DocumentBuildingData> DocumentsBuildingData) : base(DateStartProcess, DateEndProcess)
    {
        this.SectionBuildingDataName = SectionBuildingDataName;
        this.DocumentsBuildingData = DocumentsBuildingData;
    }
    public string SectionBuildingDataName { get; set; }
    public List<DocumentBuildingData> DocumentsBuildingData { get; set; }
    //public int NumberErrorsDocumentsSection => DocumentsBuildingData.Sum(x=>x.NumberErrorsEntitiesDocument);
}

public class DocumentBuildingData : MeasuringTime
{
    public DocumentBuildingData() { }
    public DocumentBuildingData(DateTime DateStartProcess, 
        DateTime DateEndProcess, 
        string DocumentTitle,
        string DocumentSection,
        List<EntityBuildingData> EntityBuildingData,
        DeletingZonesResult analyzeZonesResult) : base(DateStartProcess, DateEndProcess)
    {
        this.DocumentTitle = DocumentTitle;
        this.DocumentSection = DocumentSection;
        this.EntityBuildingData = EntityBuildingData;
        this.AnalyzeZonesResult = analyzeZonesResult;
    }
    public string DocumentTitle { get; set; }
    public string DocumentSection { get; set; }
    public List<EntityBuildingData> EntityBuildingData { get; set; }
    public DeletingZonesResult AnalyzeZonesResult { get; set; }
    //public int NumberErrorsEntitiesDocument => EntityBuildingData.Sum(x=> x.NotCreatedElementsData.BadNotCreatedElements.Count);
}

#region EntityBuildingData
public class EntityBuildingData : MeasuringTime
{
    public EntityBuildingData() { }
    public EntityBuildingData(DateTime DateStartProcess, 
        DateTime DateEndProcess, 
        string EntityName, 
        List<ElementEntity> HostElements,
        List<ElementEntity> CreatedElements,
        NotCreatedElementsData  NotCreatedElementsData) : base(DateStartProcess, DateEndProcess)
    {
        this.EntityName = EntityName;
        this.HostElements = HostElements;
        this.CreatedElements = CreatedElements;
        this.NotCreatedElementsData = NotCreatedElementsData;
    }
    public string EntityName { get; set; }
    public List<ElementEntity> HostElements { get; set; }
    public List<ElementEntity> CreatedElements { get; set; }
    public NotCreatedElementsData  NotCreatedElementsData { get; set; }
}

public class NotCreatedElementsData
{
    public NotCreatedElementsData() { }
    public NotCreatedElementsData(List<NotCreatedElementError> GoodNotCreatedElements,
        List<NotCreatedElementError> BadNotCreatedElements)
    {
        this.BadNotCreatedElements = BadNotCreatedElements;
        this.GoodNotCreatedElements = GoodNotCreatedElements;
    }
    public List<NotCreatedElementError> GoodNotCreatedElements { get; set; }
    public List<NotCreatedElementError> BadNotCreatedElements { get; set; }
}
public class NotCreatedElementError : ElementError
{
    public NotCreatedElementError() { }
    public NotCreatedElementError(string BadElementId, 
        string ReasonNotCreated) : base(BadElementId)
    {
        this.BadElementId  = BadElementId;
        this.ReasonNotCreated = ReasonNotCreated;
    }
    public string BadElementId { get; set; }
    public string ReasonNotCreated { get; set; }
}
public class DocumentNotHandledError :  ElementError
{
    public DocumentNotHandledError() { }
    public DocumentNotHandledError(string ReasonNotCreated, string modelPath) : base(ReasonNotCreated)
    {
        this.ReasonNotCreated = ReasonNotCreated;
        this.ModelPath = modelPath;
    }
    public string ReasonNotCreated { get; set; }
    public string ModelPath { get; set; }
}
public abstract class ElementError
{
    public ElementError() { }
    public ElementError(string ReasonNotCreated)
    {
        this.ReasonNotCreated = ReasonNotCreated;
    }
    public string ReasonNotCreated { get; set; }
}
#endregion

#region DeletingZones

public class DeletingZonesResult
{
    public DeletingZonesResult() { }
    public DeletingZonesResult(List<ElementEntity> SavedOldZones, 
        int DeletedOldZones, 
        List<ElementEntity> NotDeletedBusyOldZones, 
        List<ElementEntity> CreatedNewZones)
    {
        this.CreatedNewZones = CreatedNewZones;
        this.DeletedOldZones = DeletedOldZones;
        this.NotDeletedBusyOldZones = NotDeletedBusyOldZones;
        this.SavedOldZones = SavedOldZones;
    }
    public List<ElementEntity> SavedOldZones { get; set; }
    public int DeletedOldZones { get; set; }
    public List<ElementEntity> NotDeletedBusyOldZones { get; set; }
    public List<ElementEntity> CreatedNewZones { get; set; }
}

public class ElementEntity
{
    public ElementEntity() { }
    public ElementEntity(string ElementId)
    {
        this.ElementId = ElementId;
    }
    public string ElementId { get; private set; }
}
#endregion