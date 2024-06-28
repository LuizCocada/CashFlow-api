using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpenseRepository : IExpenseReadOnlyRepository,
                                   IExpenseWriteOnlyRepository,
                                   IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context;

    public ExpenseRepository( CashFlowDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task add( Expense expense )
    {
        await _context.Expenses.AddAsync( expense );// aqui prepara a despesa para ser adicionada no banco
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _context.Expenses.AsNoTracking().ToListAsync();// aqui faz o select no banco 
    }


    public async Task<bool> delete( long id )
    {
       var result = await _context.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        if( result is null )
        {
            return false;
        }

        _context.Expenses.Remove( result );

        return true;
    }
    
    //responde essa se a funcao de pegar o Id vier do ReadOnly
    async Task<Expense?>  IExpenseReadOnlyRepository.GetById( long id )
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);                 //A diferença é que um usa o AsNoTracking() e outro não.
    }

    public async Task<List<Expense>> GetByDate(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;//devolve às meia noite do primeiro dia do mes e ano passado.
        
        int daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);//verificar quando o mes acaba.
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);
        
        return await _context.Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ToListAsync();
    }

    //responde essa se a funcao de pegar o Id vier do Update                                                            // AsNoTracking é para melhor desempenho. caso o useCase nao faça alteracoes de valores, usamos ele.
    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById( long id )
    {
        return await _context.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }
    
    public void Update(Expense expense)
    {
        _context.Expenses.Update(expense);
    }
}

