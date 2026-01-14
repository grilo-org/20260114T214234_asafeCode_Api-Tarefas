using Bogus;
using TarefasCrud.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(req => req.Name, (f) => f.Person.FirstName)
            .RuleFor(req => req.Email, (f) => f.Internet.Email());
    }
}