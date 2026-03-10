using System.Text.Json;
using AutoMapper;
using BimManagerPortal.Domain.Entities.BigDataPlugins;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;

public class PostPluginBigDataCommandMapping : Profile
{
    public PostPluginBigDataCommandMapping()
    {
        CreateMap<PostPluginBigDataCommand, PluginBigData>()
            .ForMember(d => d.JsonData,
                opt => 
                    opt.MapFrom(s => 
                        JsonSerializer.Serialize(s.PostPluginConfigurationRequestDto.JsonData)
                        ));
    }
}