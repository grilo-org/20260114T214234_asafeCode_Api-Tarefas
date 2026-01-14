using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class UnauthorizedException : TarefasCrudException
{
    public UnauthorizedException(string messages) : base(messages){}
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    public override IList<string> GetErrorMessages() => [Message];
}