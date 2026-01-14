using FluentValidation.Results;
using Mapster;
using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Token;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Security.Criptography;
using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Exceptions;

namespace TarefasCrud.Application.UseCases.User.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository  _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenRepository _tokenRepository;
    public RegisterUserUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository, 
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator tokenGenerator, 
        IRefreshTokenGenerator refreshTokenGenerator, 
        ITokenRepository tokenRepository)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenRepository = tokenRepository;
    }
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await ValidateAsync(request);
        var user = request.Adapt<Domain.Entities.User>();
        
        user.UserId = Guid.NewGuid();
        user.Password = _passwordEncripter.Encrypt(request.Password);
        
        await _userWriteOnlyRepository.Add(user);
        await _unitOfWork.Commit();

        var refreshToken = await CreateAndSaveRefreshToken(user);
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _tokenGenerator.Generate(user.UserId),
                RefreshToken = refreshToken
            }
        };
    }
    
    private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
    {
        var refreshToken = new Domain.Entities.RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = user.Id
        };

        await _tokenRepository.SaveNewRefreshToken(refreshToken);

        await _unitOfWork.Commit();

        return refreshToken.Value;
    }

    private async Task ValidateAsync(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);
        
        if (result.IsValid.IsFalse())
            HandleValidationResult.ThrowError(result);
        
        var emailExists = await _userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);
        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            HandleValidationResult.ThrowError(result);
        }
    }
}