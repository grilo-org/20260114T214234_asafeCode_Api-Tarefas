using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class NotFoundException : TarefasCrudException
{
    public NotFoundException(string message) : base(message){}
    
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;

    public override IList<string> GetErrorMessages() => [Message];
}