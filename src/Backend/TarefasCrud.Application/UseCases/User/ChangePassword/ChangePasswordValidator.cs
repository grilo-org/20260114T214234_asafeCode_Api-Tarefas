using FluentValidation;
using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;

namespace TarefasCrud.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}