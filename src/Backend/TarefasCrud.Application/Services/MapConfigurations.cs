using Mapster;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Application.Services;

public static class MapConfigurations
{
    public static void Configure()
    {
        RequestToDomain();
    }
    private static void RequestToDomain()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig().Ignore(user => user.Password);
    }
}
