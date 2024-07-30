using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CashFlow.Application.useCase.Login;

public class LoginUsecase : ILoginUsecase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginUsecase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator
    )
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLogin request)
    {
        var user = await _repository.GetUserByEmail(request.Email);
        
        if (user is null)
        {
            throw new authenticationException();
        }

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if (passwordMatch is false)
        {
            throw new authenticationException();
        }

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}