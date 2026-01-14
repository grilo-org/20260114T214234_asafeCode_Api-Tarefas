using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Token;

namespace TarefasCrud.Infrastructure.DataAccess.Repositories;

public class TokenRepository :  ITokenRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TokenRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;

    public async Task<RefreshToken?> Get(string refreshToken) => await _dbContext
        .RefreshTokens
        .AsNoTracking()
        .Include(token => token.User)
        .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
    
    public async Task SaveNewRefreshToken(RefreshToken refreshToken)
    { 
        var tokens = _dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);
        _dbContext.RefreshTokens.RemoveRange(tokens);
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    } 
}