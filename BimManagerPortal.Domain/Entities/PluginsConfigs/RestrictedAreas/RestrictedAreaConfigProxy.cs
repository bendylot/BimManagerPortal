using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Domain.Entities.PluginsConfigs.RestrictedAreas;

public class RestrictedAreaConfigProxy
{
    [ValidateComplexType]
    public BuildingMode BuildingMode { get; set; } = new();
    
    [ValidateComplexType]
    public PathsToModelsProxy PathsToModels { get; set; } = new();
    
    [ValidateComplexType]
    public DisciplinesProxy Razdels { get; set; } = new();
}
public class BuildingMode
    {
        public string HandlerOldZones { get; set; } = "";
        public bool BuilderNewZones { get; set; } = true;
        public bool CreateRestrictedZoneFile { get; set; } = true;
    }
    public class DisciplinesProxy
    {
        [ValidateComplexType]
        public ArDisciplineProxy ArDiscipline { get; set; } = new();
        
        [ValidateComplexType]
        public KrDisciplineProxy KrDiscipline { get; set; } = new();
        
        [ValidateComplexType]
        public VisDisciplineProxy VisDiscipline { get; set; } = new();
    }

    #region disciplines
    public class ArDisciplineProxy
    {
        [ValidateComplexType]
        public ArEnableFormSettings ArEnableFormSettings { get; set; } = new();
    }
    public class KrDisciplineProxy
    {
        [ValidateComplexType]
        public KrEnableFormSettings KrEnableFormSettings { get; set; } = new();
    }
    public class VisDisciplineProxy
    {
        [ValidateComplexType]
        public VisEnableFormSettings VisEnableFormSettings { get; set; } = new();
    }
    #endregion
    public class ArEnableFormSettings
    {
        public bool RzArDoors { get; set; } = false;
        public bool RzRoomProperties { get; set; } = false;
        public bool RzArParking { get; set; } = false;
        public bool RzArDoorsReinforcedConcrete { get; set; } = false;
        public bool RzArDoorOpenings { get; set; } = false;
    }
    public class KrEnableFormSettings
    {
        public bool RzKrStructuralColumns { get; set; } = false;
        public bool RzKrStructuralFraming { get; set; } = false;
        public bool RzKrColumnHeads { get; set; } = false;
        public bool RzKrDoors { get; set; } = false;
        public bool RzKrWalls { get; set; } = false;
        public bool RzKrFloors { get; set; } = false;
        public bool RzKrAroundColumnsInFloor { get; set; } = false;
        public bool RzKrAroundWallsInFloor { get; set; } = false;
        public bool RzKrUnderColumnInWalls { get; set; } = false;
    }
    public class VisEnableFormSettings
    {
        public bool RzSsCableTray { get; set; } = false;
        public bool RzEomCableTray { get; set; } = false;
        public bool RzEomDuctRectangle { get; set; } = false;
        public bool RzPipeVk { get; set; } = false;
        public bool RzPipePt { get; set; } = false;
        public bool RzPipeOt { get; set; } = false;
        public bool RzPipeKv { get; set; } = false;
        public bool RzDuctVentRound { get; set; } = false;
        public bool RzDuctVentRectangle { get; set; } = false;
        public bool RzDuctSsRectangle { get; set; } = false;
    }
    public class PathsToModelsProxy
    {
        [StringLength(10, MinimumLength = 1,ErrorMessage = "Длина от 1 до 10 символов")]
        public string ObjectName { get; set; } = "";
        
        [RegularExpression(@"^[a-zA-Z]:\\([^<>:""/\\|?*\r\n]+\\)*[^<>:""/\\|?*\r\n]*$",ErrorMessage = "Некорректный путь Windows")]
        [StringLength(100, MinimumLength = 1,ErrorMessage = "Длина от 1 до 100 символов")]
        public string TemplatePath { get; set; } = "";
        
        [RegularExpression(@"^[a-zA-Z]:\\([^<>:""/\\|?*\r\n]+\\)*[^<>:""/\\|?*\r\n]*$",ErrorMessage = "Некорректный путь Windows")]
        [StringLength(100, MinimumLength = 1,ErrorMessage = "Длина от 1 до 100 символов")]
        public string PathForFilesWithOnlyRestrictedAreas { get; set; } = "";
        
        [StringLength(20, MinimumLength = 0,ErrorMessage = "Длина от 0 до 20 символов")]
        public string PrefixForFilesWithOnlyRestrictedAreas { get; set; } = "";
        
        [StringLength(20, MinimumLength = 0,ErrorMessage = "Длина от 0 до 20 символов")]
        public string SuffixForFilesWithOnlyRestrictedAreas { get; set; } = "_Запретные зоны";
        
        public List<Model> Models { get; set; } = new();
    }
    public class Model
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [RegularExpression(@"^[a-zA-Z]:\\([^<>:""/\\|?*\r\n]+\\)*[^<>:""/\\|?*\r\n]*$",ErrorMessage = "Некорректный путь Windows")]
        [StringLength(100, MinimumLength = 1,ErrorMessage = "Длина от 1 до 100 символов")]
        public string ModelPath { get; set; }
    }