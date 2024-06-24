using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.useCase.Expenses.Registered.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpenseUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase( IUnitOfWork unitOfWork, IMapper mapper, IExpenseUpdateOnlyRepository repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task Execute(long id, RequestExpenseJson request )         
    {
        Validate(request);

        var expense = await _repository.GetById(id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }
            
        //passa o que veio de request para expense
        _mapper.Map(request, expense);
        
        _repository.Update(expense);

        await _unitOfWork.commit();
    }

    private void Validate(RequestExpenseJson request) 
    {
        var validator = new ExpensesValidation();

        var result = validator.Validate(request);

        if (result.IsValid is false) 
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorValidationException(errorMessages);
        }
    }
}

