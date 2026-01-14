using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException : TarefasCrudException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION){}
    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}