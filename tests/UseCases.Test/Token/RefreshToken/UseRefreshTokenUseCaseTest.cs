using CommonTestUtilities.Entities;
using CommonTestUtilities.Providers;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Tokens;
using CommonTestUtilities.Tokens;
using CommonTestUtilities.ValueObjects;
using Shouldly;
using TarefasCrud.Application.UseCases.Token.RefreshToken;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.ValueObjects;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.Token.RefreshToken;

public class UseRefreshTokenUseCaseTest
{
     [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);

        var useCase = CreateUseCase(refreshToken);
        var request = new RequestNewTokenJson { RefreshToken = refreshToken.Value };
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.AccessToken.ShouldNotBeNullOrEmpty();
        result.RefreshToken.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_RefreshToken_Not_Found()
    {
        var useCase = CreateUseCase();

        var request = new RequestNewTokenJson { RefreshToken = string.Empty };
        var act = async () => await useCase.Execute(request);
        
        var exception = await act.ShouldThrowAsync<RefreshTokenNotFoundException>();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.EXPIRED_SESSION);
    }

    [Fact]
    public async Task Error_RefreshToken_Expired()
    {
        var (user, _) = UserBuilder.Build();
        var refreshToken = RefreshTokenBuilder.Build(user);
        refreshToken.CreatedOn = TarefasCrudTestsConstants.DateForTests.AddDays(-TarefasCrudRuleConstants.REFRESH_TOKEN_EXPIRATION_DAYS - 1);
        
        var useCase = CreateUseCase(refreshToken);
        var request = new RequestNewTokenJson { RefreshToken = refreshToken.Value };
        var act = async () => await useCase.Execute(request);
        
        var exception = await act.ShouldThrowAsync<RefreshTokenExpiredException>();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.INVALID_SESSION);
    }

    private static UseRefreshTokenUseCase CreateUseCase(TarefasCrud.Domain.Entities.RefreshToken? refreshToken = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
        var tokenRepository = new TokenRepositoryBuilder().Get(refreshToken).Build();
        
        return new UseRefreshTokenUseCase(tokenRepository, unitOfWork, accessTokenGenerator, refreshTokenGenerator);
    }
}