using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.useCase.Expenses.Registered.GetAll;

public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{

    private readonly IExpenseReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllExpenseUseCase( IExpenseReadOnlyRepository repository , IMapper mapper )
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpensesJson> Execute()
    {
        var getAll = await _repository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(getAll)
        };
    }
}

