using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.RoutineTask.Register;

public interface IRegisterTaskUseCase
{
    public Task<ResponseRegisteredTaskJson> Execute(RequestTaskJson request);
}