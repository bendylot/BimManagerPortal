namespace BimManagerPortal.Shared.Dtos.ErrorDictionary;

public class CreateErrorDictionaryEntryRequestDto
{
    public int DictionaryType { get; set; }
    public string KeyPhrase { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string UserCreater { get; set; } = string.Empty;
}
