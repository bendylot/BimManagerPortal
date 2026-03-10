using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Shared.Dtos.PluginBigDatas;

public class GetAllPluginBigDatasDto
{
    public GetAllPluginBigDatasDto(string Id,
        string PluginName,
        //JsonElement JsonData,
        string UserCreater,
        string CreatedAt)
    {
        this.Id = Id;
        this.PluginName = PluginName;
        this.CreatedAt = CreatedAt;
        //this.JsonData = JsonData;
        this.UserCreater = UserCreater;
    }
    [Required]
    public string Id { get; set; }
    [Required]
    public string PluginName { get; set; }
    /*[Required]
    public JsonElement JsonData { get; set; }*/
    [Required]
    public string UserCreater { get; set; }
    [Required]
    public string CreatedAt { get; set; }
    /*public void Mapping(Profile profile)
    {
        profile.CreateMap<PluginBigData, GetAllPluginBigDatasDto>();
    }*/
}