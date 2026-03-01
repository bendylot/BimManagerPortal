using BimManagerPortal.Domain.Entities.PluginsConfigs.RestrictedAreas;
using Microsoft.AspNetCore.Components;

namespace BimManagerPortal.WebBlazorSite.UIComponents.PluginConfigurations.Tabs.CreateConfigurations.Forms.RestrictedAreaForm;

public partial class RestrictedAreaForm
{
    [Parameter] public RestrictedAreaConfigProxy Config { get; set; } = default!;
    
    #region private
    private void HandleValidSubmit()
    {
        Console.WriteLine("Данные прошли валидацию");
    }
    private void AddModel(PathsToModelsProxy paths)
    {
        paths.Models ??= new List<Model>();
        paths.Models.Add(new Model { ModelPath = "" });
    }

    private void RemoveModel(PathsToModelsProxy paths, int index)
    {
        if (paths.Models != null && index >= 0 && index < paths.Models.Count)
        {
            paths.Models.RemoveAt(index);
        }
    }
    #endregion
    
}