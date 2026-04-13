using BimManagerPortal.Domain.Common;

namespace BimManagerPortal.Domain.Entities.ErrorDictionary;

public class ErrorDictionaryEntry : BaseAuditableEntity
{
    protected ErrorDictionaryEntry() { }

    public ErrorDictionaryEntry(
        string userCreater,
        ErrorDictionaryType dictionaryType,
        string keyPhrase,
        string explanation) : base(userCreater)
    {
        DictionaryType = dictionaryType;
        KeyPhrase = keyPhrase;
        Explanation = explanation;
    }

    public ErrorDictionaryType DictionaryType { get; set; }
    public string KeyPhrase { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
}
