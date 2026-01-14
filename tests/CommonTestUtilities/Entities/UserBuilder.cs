using Bogus;
using CommonTestUtilities.Cryptography;
using TarefasCrud.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static (User user, string password) Build()
    {
        var passwordEncripter =  PasswordEncripterBuilder.Build();
        var password = new Faker().Internet.Password();

        var user = new Faker<User>()
            .RuleFor(user => user.Name, (f) => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.UserId, (_) => Guid.NewGuid())
            .RuleFor(user => user.Password, (_) => passwordEncripter.Encrypt(password));
        
        return (user, password);
    }
}