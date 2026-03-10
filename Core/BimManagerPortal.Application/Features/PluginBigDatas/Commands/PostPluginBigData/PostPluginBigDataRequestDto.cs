using System.Text.Json;
using AutoMapper;
using BimManagerPortal.Application.Mappings;
using BimManagerPortal.Domain.Entities.BigDataPlugins;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;

public class PostPluginBigDataRequestDto : IMapFrom<PluginBigData>
{
    public PostPluginBigDataRequestDto() {}

    public PostPluginBigDataRequestDto(string PluginName, JsonElement JsonData, string UserCreater)
    {
        this.PluginName = PluginName;
        this.JsonData = JsonData;
        this.UserCreater = UserCreater;
    }
    public string PluginName { get; set; }
    public JsonElement? JsonData { get; set; }
    public string UserCreater { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<PostPluginBigDataRequestDto, PluginBigData>()
            .ForMember(d => d.JsonData,opt => opt.MapFrom(s => JsonSerializer.Serialize(s.JsonData)))
            .ForMember(d => d.PluginName,opt => opt.MapFrom(s => s.PluginName)) // Явное указание маппинга
                .ForMember(d => d.UserCreater, 
                    opt => opt.MapFrom(s => s.UserCreater));
    }
}