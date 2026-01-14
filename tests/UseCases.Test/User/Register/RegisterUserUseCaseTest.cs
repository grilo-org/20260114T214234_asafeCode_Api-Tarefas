using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Tokens;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Application.UseCases.User.Register;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;
using TarefasCrud.Infrastructure.Security.Tokens.Refresh;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();
        var result = await useCase.Execute(request);
        
        result.ShouldNotBeNull().ShouldSatisfyAllConditions(() =>
        {
            result.Name.ShouldBeSameAs(request.Name); result.Name.ShouldNotBeNullOrWhiteSpace();
            result.Tokens.ShouldNotBeNull(); result.Tokens.AccessToken.ShouldNotBeNullOrEmpty();
            result.Tokens.RefreshToken.ShouldNotBeNullOrEmpty();
        });
    }
    
    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);
        
        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    } 
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();
        
        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.NAME_EMPTY);
    }
    
    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var writeOnly = UserWriteOnlyRepositoryBuilder.Build();   
        var readOnlyBuilder = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Build();

        if (email.NotEmpty())
        {
            readOnlyBuilder.ExistsActiveUserWithEmail(email);
        }
        
        return new RegisterUserUseCase(writeOnly, readOnlyBuilder.Build(), unitOfWork, passwordEncripter, accessTokenGenerator, refreshTokenGenerator, tokenRepository);
    }
}