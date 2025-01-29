using System.Net;


namespace HttpModels.Responses
{
    public record ErrorResponse(string Message, int? StatusCode = null);
}
