using System.Net;

namespace TarefasCrud.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : TarefasCrudException
{
    private readonly IList<string> _errorMessages;

    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty)
    {
        _errorMessages = errorMessages;
    }

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;


    public override IList<string> GetErrorMessages() => _errorMessages;
}
