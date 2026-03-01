namespace BimManagerPortal.Domain.Entities.PluginsConfigs.RestrictedAreas
{
    public class RestrictedAreaConfigProxy
    {
        public string ObjectName { get; set; } = "";
        public DisciplinesProxy Razdels { get; set; } = new();
    }

    public class DisciplinesProxy
    {
        public ArDisciplineProxy ArDiscipline { get; set; } = new();
        public KrDisciplineProxy KrDiscipline { get; set; } = new();
        public VisDisciplineProxy VisDiscipline { get; set; } = new();
    }

    #region disciplines
    public class ArDisciplineProxy
    {
        public ArEnableFormSettings ArEnableFormSettings { get; set; } = new();
        public PathsToModelsProxy PathsToModels { get; set; } = new();
    }
    public class KrDisciplineProxy
    {
        public KrEnableFormSettings KrEnableFormSettings { get; set; } = new();
        public PathsToModelsProxy PathsToModels { get; set; } = new();
    }
    public class VisDisciplineProxy
    {
        public VisEnableFormSettings VisEnableFormSettings { get; set; } = new();
        public PathsToModelsProxy PathsToModels { get; set; } = new();
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
        public string TemplatePath { get; set; } = "";
        public string PathForFilesWithOnlyRestrictedAreas { get; set; } = "";
        public string PrefixForFilesWithOnlyRestrictedAreas { get; set; } = "";
        public string SuffixForFilesWithOnlyRestrictedAreas { get; set; } = "";
        public bool ForcedRebuilding { get; set; } = false;
        public List<Model> Models { get; set; } = new();
    }
    public class Model
    {
        public string ModelPath { get; set; }
    }
}
