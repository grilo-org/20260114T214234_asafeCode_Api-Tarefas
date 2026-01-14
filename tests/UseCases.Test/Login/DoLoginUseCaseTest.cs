using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Tokens;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Application.UseCases.Login;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });

        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldNotBeNullOrWhiteSpace();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<InvalidLoginException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);
    }

    private static DoLoginUseCase CreateUseCase(TarefasCrud.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetUserByEmail(user);

        return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter, accessTokenGenerator, refreshTokenGenerator, tokenRepository, unitOfWork);
    }
}