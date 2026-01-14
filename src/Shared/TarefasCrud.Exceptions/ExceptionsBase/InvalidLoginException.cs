using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class InvalidLoginException : TarefasCrudException
{
    public InvalidLoginException() : base(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID){}
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    public override IList<string> GetErrorMessages() => [Message];
}