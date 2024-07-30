using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;

namespace CashFlow.Application.useCase.Users.Register;

public class RegisterUserValidation : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidation()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()//só irá executar se a condicao do WHEN abaixo for verdadeira.
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)//so ira executar se user.email nao for nulo, ou seja email preenchido mas inválido.
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        RuleFor(user => user.Password).SetValidator(new PasswordValidation<RequestRegisterUserJson>());
    }
}



//Criamos uma classe externa para o Password, para quando implementarmos a opção de mudar senha nao repetirmos codigos.