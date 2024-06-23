using AutoMapper;
using CashFlow.Application.useCase.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.useCase.Expenses.Registered.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase( IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    public async Task Execute(long id, RequestExpenseJson request )
    {
        Validate(request);

        await _unitOfWork.commit();
    }

    public void Validate(RequestExpenseJson request) 
    {
        var validator = new RegisterExpensesValidation();

        var result = validator.Validate(request);

        if (result.IsValid == false) 
        {
            var errorMessages = result.Errors.Select(E => E.ErrorMessage).ToList();

            throw new ErrorValidationException(errorMessages);
        }
    }
}

//MINUTO 10;