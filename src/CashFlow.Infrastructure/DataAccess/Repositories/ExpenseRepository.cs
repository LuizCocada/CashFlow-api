using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpenseRepository : IExpenseReadOnlyRepository, IExpenseWriteOnlyRepository
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

    public async Task<Expense?> GetById( long id )
    {
        return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
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
}


// AsNoTracking é para melhor desempenho. caso o useCase nao faça alteracoes de valores, usamos ele.