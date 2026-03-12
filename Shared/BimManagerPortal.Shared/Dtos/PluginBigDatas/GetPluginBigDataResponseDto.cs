using System.Text.Json;

namespace BimManagerPortal.Shared.Dtos.PluginBigDatas;

public class GetPluginBigDataResponseDto
{
    public GetPluginBigDataResponseDto(JsonElement Json)
    {
        this.Json = Json;
    }
    public JsonElement Json { get; set; }
}