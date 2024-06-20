using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.useCase.Expenses.Register;

public class RegisterExpensesValidation : AbstractValidator<RequestRegisterExpenseJson>
{
    public RegisterExpensesValidation()
    {
        //verifica se nao � nulo. Se for nulo retorna uma mensagem de tratamento.
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_EMPTY);

        //verifica se o valor � maior que zero. Se for menor ou igual a zero retorna mensagem de tratamento.
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.INVALID_AMOUNT);

        //Verifica se a data da despesa � menor ou igual a zero. se for maior significa que ele esta programando uma despesa para o futuro, o que nao pode.
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);

        //verifica se o valor recebido no parametro existe dentro do Enum.
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_PAYMENT_TYPE);

    }
}

