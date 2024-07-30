using CashFlow.Application.useCase.Login;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Login;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace Usecases.Tests.Login;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var request = LoginBuilder.Build();
        
        request.Email = user.Email; //estou fazendo isto, pois estou passando o USER como parametro para o mock de GetUserByEmail
                                    //e REQUEST para verificar no LoginuseCase. Os dois devem ser o mesmos valores.
        
        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task User_Not_Found()
    {
        var user = UserBuilder.Build();
        
        var request = LoginBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = act.Should().ThrowAsync<authenticationException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVALID_EMAIL_OR_PASSWORD));
    }
    
    [Fact]
    public async Task Password_Not_Match()
    {
        var user = UserBuilder.Build();
        
        var request = LoginBuilder.Build();

        request.Email = user.Email;
        
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);
        
        var result = act.Should().ThrowAsync<authenticationException>();
        result.Where(ex =>
            ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVALID_EMAIL_OR_PASSWORD));
    }
    
    

    private LoginUsecase CreateUseCase(User user, string? password = null)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
        var accesToken = JwtTokenGeneratorBuilder.Build();
        var userReadOnly = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
        
        return new LoginUsecase(userReadOnly, passwordEncrypter, accesToken);
    }
}

//só é possivel fazer este encadeamento com this dentro da classe UserReadOnlyRepositoryBuilder;


//se retornasse void na funcao GetUserByEmail fariamos assim => 
// var userReadOnly = new UserReadOnlyRepositoryBuilder();
// userReadOnly.GetUserByEmail(user);
// return new LoginUsecase(userReadOnly.Build(), passwordEncrypter, accesToken);