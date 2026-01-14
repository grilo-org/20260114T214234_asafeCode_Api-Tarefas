using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.User;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public UserRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
    
    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext
        .Users
        .AnyAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<User?> GetUserByEmail(string email) => await _dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userId) => await _dbContext
        .Users
        .AnyAsync(user => user.UserId.Equals(userId) && user.Active);

    async Task<User?> IUserReadOnlyRepository.GetByUserIdentifier(Guid userId) => await _dbContext
        .Users
        .AsNoTracking()
        .FirstOrDefaultAsync(user => user.UserId.Equals(userId) && user.Active);

    async Task<User> IUserUpdateOnlyRepository.GetUserById(long id) => await _dbContext
        .Users
        .FirstAsync(user => user.Id.Equals(id) && user.Active);
    
    public void Update(User user) => _dbContext.Users.Update(user);
}