using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
         _repository.Setup(config => config.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(config => config.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}


//_repository.Setup(config => config.ExistActiveUserWithEmail(It.IsAny<string>())).ReturnsAsync(true); =>
    //Só vai retornar TRUE se o email passado no parametro for o email da request.

//Se estiver chamando um repositorio que a funcao é uma TASK o metodo de retorno utilizado deve ser returnAsync

//PRECISA RETORNAR TRUE NESTE CASO, POIS NA VALIDACAO DO USECASE SO ENTRA NO ExistEmail se for True.

//OBS: o valor padrao para boolean é FALSE.

//-------------------------

 // Executa a linha do repositorio, e logo em seguida retorna a instancia da classe UserReadOnlyRepositoryBuilder() e todos os seus metodos, incluindo
//incluindo o Builder()

    // public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    // {
    //     _repository.Setup(config => config.GetUserByEmail(user.Email)).ReturnsAsync(user);
    //
    //   return this;
    //  }


