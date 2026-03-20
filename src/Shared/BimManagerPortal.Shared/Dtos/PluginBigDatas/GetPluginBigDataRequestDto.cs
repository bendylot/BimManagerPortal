namespace BimManagerPortal.Shared.Dtos.PluginBigDatas;

public class GetPluginBigDataRequestDto
{
    public GetPluginBigDataRequestDto(string Id)
    {
        this.Id = Id;
    }
    public string Id { get; set; }
}