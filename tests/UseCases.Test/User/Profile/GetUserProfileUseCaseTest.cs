using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using Shouldly;
using TarefasCrud.Application.UseCases.User.Profile;

namespace UseCases.Test.User.Profile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute();
        
        result.ShouldNotBeNull().ShouldSatisfyAllConditions(() =>
        {
            result.Name.ShouldBeSameAs(user.Name);
            result.Email.ShouldBeSameAs(user.Email);
        });
    }
    
    
    private static GetUserProfileUseCase CreateUseCase(TarefasCrud.Domain.Entities.User? user = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user!);
        
        return new GetUserProfileUseCase(loggedUser);
    }
}