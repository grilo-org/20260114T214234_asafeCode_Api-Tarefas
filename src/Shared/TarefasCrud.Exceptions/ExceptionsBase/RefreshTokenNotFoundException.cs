using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class RefreshTokenNotFoundException : TarefasCrudException
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesException.EXPIRED_SESSION){}
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    public override IList<string> GetErrorMessages() => [Message];
}