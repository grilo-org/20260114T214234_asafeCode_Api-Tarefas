using Bogus;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Entities;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(f => f.Name, f => f.Name.FirstName())
            .RuleFor(f => f.Email, f => f.Internet.Email())
            .RuleFor(f => f.Password, f => f.Internet.Password(passwordLength));
    }
}