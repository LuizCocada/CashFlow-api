using CashFlow.Application.useCase.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace Usecases.Tests.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);//duvidas final do arquivo
        var result = await act.Should().ThrowAsync<ErrorValidationException>();//executa a função armazenada em act e esta função deve retortar uma exception do tipo ErrorValidation
        
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));//validação extra.
    }

    [Fact]
    public async Task Error_Email_Alredy_Exist()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);
        var result = act.Should().ThrowAsync<ErrorValidationException>();

        await result.Where(ex => ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }
    
    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var PasswordEncrypter = new PasswordEncrypterBuilder().Build();
        
        var readOnly = new UserReadOnlyRepositoryBuilder();
        if (string.IsNullOrWhiteSpace(email) == false)
        {
            readOnly.ExistActiveUserWithEmail(email);
        }
        
        var writeOnly = UserWriteOnlyRepositoryBuilder.Build();
        var jwtToken = JwtTokenGeneratorBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new RegisterUserUseCase(mapper, PasswordEncrypter, readOnly.Build(), writeOnly, jwtToken , unitOfWork);
    }
}





//--------H-E-L-P--------
//  var act = async () => await useCase.Execute(request);
//  isto é igual fazer isto => 
//  public async Task Act()
//  {
//      var request = RequestRegisterUserJsonBuilder.Build();
//      request.Name = string.Empty;
// 
//      await useCase = CreateUseCase();
//  }
    
//Por que nao estamos fazendo assim? => 
//  var result = await useCase.Execute(request);
//Por que em UseCase, ele irá executar um Throw quando o usuario for invalid.
    
// fazendo desta forma acima, a variavel/funcao action(act), executa o useCase e espera um error do tipo
//ErrorValidationException