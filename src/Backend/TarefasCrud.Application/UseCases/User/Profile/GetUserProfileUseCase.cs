using Mapster;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Services.LoggedUser;

namespace TarefasCrud.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    public GetUserProfileUseCase(ILoggedUser loggedUser) => _loggedUser = loggedUser;

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();
        return user.Adapt<ResponseUserProfileJson>();
    }
}