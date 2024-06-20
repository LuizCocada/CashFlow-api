using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.useCase.Expenses.Registered.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase( IExpenseReadOnlyRepository repository , IMapper mapper )
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseByIdJson> Execute(long id)
    {
        var result = await _repository.GetById( id );

        if ( result is null )
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        return _mapper.Map<ResponseExpenseByIdJson>(result);
    }
}
