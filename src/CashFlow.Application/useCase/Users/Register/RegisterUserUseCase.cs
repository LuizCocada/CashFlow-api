using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.useCase.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserUseCase(
        IMapper mapper,
        IPasswordEncripter passwordEncripter,
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    { 
       await Validade(request);
        
        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _writeOnlyRepository.Add(user);
        await _unitOfWork.commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }

    private async Task Validade(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidation();
        var result = validator.Validate(request);

        var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
        {
            result.Errors.Add(new ValidationFailure(string.Empty,ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorValidationException(errorMessages);
        }
    }
}