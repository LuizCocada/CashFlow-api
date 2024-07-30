using CashFlow.Application.useCase.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
        [Fact]
    public void Success()
    {
        //Arrange
        var validator = new RegisterUserValidation();
        var request = RequestRegisterUserJsonBuilder.Build();
        //Act
        var result = validator.Validate(request);
        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new RegisterUserValidation();
        var request = RequestRegisterUserJsonBuilder.Build();
        
        request.Name = name; // o name recebido do parametro vai ser algum dos InlineData
        
        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }
    
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new RegisterUserValidation();
        var request = RequestRegisterUserJsonBuilder.Build();
        
        request.Email = email; // o name recebido do parametro vai ser algum dos InlineData
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }
    
    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidation();
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Email = "EmailSemArroba.com";
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Password_Empty(string password)
    {
        var validator = new RegisterUserValidation();
        var request = RequestRegisterUserJsonBuilder.Build();

        request.Password = password;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_INVALID));
    }
}