using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation;

namespace CashFlow.Application.useCase.Expenses.Register;

public class RegisterExpensesUseCase : IRegisterExpenseUseCase
{

    private readonly IExpenseWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterExpensesUseCase( IExpenseWriteOnlyRepository repository , IUnitOfWork unitOfWork, IMapper mapper )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ResponseRegisterExpenseJson> Execute( RequestExpenseJson request )
    {
        validate(request);

        var entity = _mapper.Map<Expense>(request);

        await _repository.add(entity);
        await _unitOfWork.commit();

        var responseRegistered = _mapper.Map<ResponseRegisterExpenseJson>(entity);

        return responseRegistered;
    }

    private void validate( RequestExpenseJson request )
    {
        var validator = new RegisterExpensesValidation();
        var result = validator.Validate(request);


        if(result.IsValid == false )
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorValidationException(errorMessages);
        }
    }
}

