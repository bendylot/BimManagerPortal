namespace BimManagerPortal.Application.Other.Exceptions;

public class SendPluginConfigException : Exception
{
    public string UserFriendlyMessage { get; }
    public System.Net.HttpStatusCode StatusCode { get; }
    public string? Detail { get; }

    public SendPluginConfigException(
        string message, 
        System.Net.HttpStatusCode statusCode, 
        string? detail = null, 
        string? userFriendlyMessage = null) 
        : base(message)
    {
        StatusCode = statusCode;
        Detail = detail;
        // Если сервер прислал Detail, используем его, иначе наше общее сообщение
        UserFriendlyMessage = userFriendlyMessage ?? detail ?? "Произошла ошибка на стороне сервера.";
    }
}