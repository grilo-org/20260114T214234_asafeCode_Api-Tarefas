using Bogus;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.ValueObjects;

namespace CommonTestUtilities.Entities;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build(User user)
    {
        return new Faker<RefreshToken>()
            .RuleFor(r => r.CreatedOn, f => f.Date.Soon(days: TarefasCrudRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS))
            .RuleFor(r => r.Value, f => f.Lorem.Word())
            .RuleFor(r => r.UserId, _ => user.Id)
            .RuleFor(r => r.User, _ => user);
    }
}