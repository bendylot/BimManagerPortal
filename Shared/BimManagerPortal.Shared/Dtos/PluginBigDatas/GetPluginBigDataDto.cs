using System.Text.Json;

namespace BimManagerPortal.Shared.Dtos.PluginBigDatas;

public class GetPluginBigDataDto
{
    public GetPluginBigDataDto(JsonElement Json)
    {
        this.Json = Json;
    }
    public JsonElement Json { get; set; }
}