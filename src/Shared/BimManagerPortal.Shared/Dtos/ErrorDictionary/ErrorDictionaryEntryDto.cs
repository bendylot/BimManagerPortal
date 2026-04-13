namespace BimManagerPortal.Shared.Dtos.ErrorDictionary;

public class ErrorDictionaryEntryDto
{
    public Guid Id { get; set; }
    public int DictionaryType { get; set; }
    public string KeyPhrase { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string UserCreater { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
