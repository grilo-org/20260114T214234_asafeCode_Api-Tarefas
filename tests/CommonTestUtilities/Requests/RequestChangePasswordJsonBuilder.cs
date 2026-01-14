using Bogus;
using TarefasCrud.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(request => request.NewPassword, (f) => f.Internet.Password(passwordLength));
    }
}