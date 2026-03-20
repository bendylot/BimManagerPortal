using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Application.Other.PluginsConfigs.RestrictedAreas;

public class RestrictedAreaConfigProxy
{
    [ValidateComplexType]
    public BuildingMode BuildingMode { get; set; } = new();
    
    [ValidateComplexType]
    public PathsToModelsProxy PathsToModels { get; set; } = new();
    
    [ValidateComplexType]
    public DisciplinesProxy DisciplineSettings { get; set; } = new();
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
        public ArDisciplineProxy ArDisciplineSetting { get; set; } = new();
        
        [ValidateComplexType]
        public KrDisciplineProxy KrDisciplineSetting { get; set; } = new();
        
        [ValidateComplexType]
        public VisDisciplineProxy VisDisciplineSetting { get; set; } = new();
    }

    #region disciplines
    public class ArDisciplineProxy
    {
        public bool ArDoors { get; set; } = false;
        public bool ArRoomProperties { get; set; } = false;
        public bool ArParking { get; set; } = false;
        public bool ArDoorsReinforcedConcrete { get; set; } = false;
        public bool ArDoorOpenings { get; set; } = false;
    }
    public class KrDisciplineProxy
    {
        public bool KrStructuralColumns { get; set; } = false;
        public bool KrStructuralFraming { get; set; } = false;
        public bool KrColumnHeads { get; set; } = false;
        public bool KrDoors { get; set; } = false;
        public bool KrWalls { get; set; } = false;
        public bool KrFloors { get; set; } = false;
        public bool KrAroundColumnsInFloor { get; set; } = false;
        public bool KrAroundWallsInFloor { get; set; } = false;
        public bool KrUnderColumnInWalls { get; set; } = false;
    }
    public class VisDisciplineProxy
    {
        public bool SsCableTray { get; set; } = false;
        public bool EomCableTray { get; set; } = false;
        public bool EomDuctRectangle { get; set; } = false;
        public bool PipeVk { get; set; } = false;
        public bool PipePt { get; set; } = false;
        public bool PipeOt { get; set; } = false;
        public bool PipeKv { get; set; } = false;
        public bool DuctVentRound { get; set; } = false;
        public bool DuctVentRectangle { get; set; } = false;
        public bool DuctSsRectangle { get; set; } = false;
    }
    #endregion
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