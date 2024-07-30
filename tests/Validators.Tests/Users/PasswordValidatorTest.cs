using CashFlow.Application.useCase.Users;
using CashFlow.Application.useCase.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;

namespace Validators.Tests.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]//entra se for null ou white-space
    [InlineData("    ")]
    [InlineData(null)]//termino
    [InlineData("a")]//entra se for menor que 8 caractere
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]//termino
    [InlineData("aaaaaaaa")]//vai ter 8 caractere porem nenhuma letra maiuscula.
    [InlineData("AAAAAAAA")]//Verdadeiro para letra maiuscula, falso p/ nenhuma letra minuscula
    [InlineData("AAAAaaaa")]//true > 8; true p/ minuscula e maiucula; false para numeros.
    [InlineData("AAAAaaaa1")]//true > 8; true p/ minuscula e maiucula; true para numeros false opara especiais.
    public void Error_Password_Empty(string password)
    {
        var validator = new PasswordValidation<RequestRegisterUserJson>();

        var result = validator
            .IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);
        
        result.Should().BeFalse();
    }
}



//para entender este INLINEDATA devemos entender que na classe PASSWORDVALIDATION so ira entrar nos if`s encadeados apenas se
//a condição anterior for verdadeira. EXEMPLO =>
//[InlineData("aa")]
// [InlineData("aaa")]
// [InlineData("aaaa")]
// [InlineData("aaaaa")]
// [InlineData("aaaaaa")]

//Aqui estamos entrando no bloco de if que diz que é menos que 8 caractere.

//[InlineData("aaaaaaaa")] => ja aqui, ele retorna verdadeiro para 8 caractere, e entra no bloco if que diz que nao tem nenhuma letra maiuscula.
//e assim vai.
