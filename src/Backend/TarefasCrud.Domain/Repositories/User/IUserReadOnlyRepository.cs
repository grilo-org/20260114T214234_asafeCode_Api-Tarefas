namespace TarefasCrud.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistsActiveUserWithEmail(string email);
    public Task<Entities.User?> GetUserByEmail(string email);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userId);
    public Task<Entities.User?> GetByUserIdentifier(Guid userId);
}