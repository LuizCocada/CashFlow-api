using System.Data;
using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IExpenseUpdateOnlyRepository
{
    Task<Expense?> GetById(long id);
    void Update(Expense expense);
}