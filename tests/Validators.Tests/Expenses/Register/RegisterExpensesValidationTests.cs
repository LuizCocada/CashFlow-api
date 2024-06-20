using CashFlow.Application.useCase.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpensesValidationTests
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterExpensesValidation();

        var request = RequestRegisterExpenseJsonBuilder.build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Title_Empty()
    {
        var validator = new RegisterExpensesValidation();
        var request = RequestRegisterExpenseJsonBuilder.build();
        request.Title = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_EMPTY));
    }

    [Fact]
    public void Error_date_future()
    {
        var validator = new RegisterExpensesValidation();
        var request = RequestRegisterExpenseJsonBuilder.build();

        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_DATE));
    }

    [Fact]
    public void Error_PaymentType_invalid()
    {
        var validator = new RegisterExpensesValidation();
        var request = RequestRegisterExpenseJsonBuilder.build();

        request.PaymentType = (PaymentType) 700;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PAYMENT_TYPE));
    }


    [Theory]//ira executar varias vezes, recebendo como paramentro 0, -1, -2, -10
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    public void Error_Amaount_less_than_or_equal_to_zero(decimal amount)
    {
        var validator = new RegisterExpensesValidation();
        var request = RequestRegisterExpenseJsonBuilder.build();

        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PAYMENT_TYPE));
    }

}

