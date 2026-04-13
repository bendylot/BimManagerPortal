namespace BimManagerPortal.Shared.Dtos.ErrorDictionary;

public class UpdateErrorDictionaryEntryRequestDto
{
    public Guid Id { get; set; }
    public string KeyPhrase { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string UserUpdater { get; set; } = string.Empty;
}
