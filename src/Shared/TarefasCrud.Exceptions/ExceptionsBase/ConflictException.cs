using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class ConflictException : TarefasCrudException
{
    public ConflictException(string messages) : base(messages){}

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Conflict;

    public override IList<string> GetErrorMessages() => [Message];
}