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


    public async Task<ResponseRegisteredExpenseJson> Execute( RequestExpenseJson request )
    {
        Validate(request);

        var entity = _mapper.Map<Expense>(request);

        await _repository.add(entity);
        await _unitOfWork.commit();

        var responseRegistered = _mapper.Map<ResponseRegisteredExpenseJson>(entity);

        return responseRegistered;
    }

    private void Validate( RequestExpenseJson request )
    {
        var validator = new ExpensesValidation();
        var result = validator.Validate(request);


        if(result.IsValid == false )
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorValidationException(errorMessages);
        }
    }
}

