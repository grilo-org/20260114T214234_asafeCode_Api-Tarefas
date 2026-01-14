using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Domain.Services.LoggedUser;

namespace TarefasCrud.Infrastructure.Services;
public class LoggedUser : ILoggedUser
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly ITokenProvider _token;
    private readonly IAccessTokenValidator _tokenValidator;

    public LoggedUser(IUserReadOnlyRepository repository, 
        ITokenProvider token, 
        IAccessTokenValidator tokenValidator)
    {
        _repository = repository;
        _token = token;
        _tokenValidator = tokenValidator;
    }
    public async Task<User> User()
    {
        var token = _token.Value();
        var userId = _tokenValidator.ValidateAndGetUserId(token);
        var loggedUser = await _repository.GetByUserIdentifier(userId);
        return loggedUser!;
    }
}