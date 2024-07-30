using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(AccessToken => AccessToken.Generate(It.IsAny<User>())).Returns("TokenGeradoMokado");

        return mock.Object;
    }
}


//este mock devolve um valor em 
//se a funcao Generete nao tivesse nenhum parametro, seria assim =>
//mock.Setup(AccessToken => AccessToken.Generate().Returns("TokenMokado");

//Já que a função possui o parametro User. este bloco It.IsAny<User>() fala que independente do parametro retorne
//a mensagem "STRING"
