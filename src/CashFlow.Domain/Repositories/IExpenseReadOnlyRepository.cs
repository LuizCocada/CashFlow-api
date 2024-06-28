using CashFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Domain.Repositories;
public interface IExpenseReadOnlyRepository
{
    Task<List<Expense>> GetAll();

    Task<Expense?> GetById( long id );

    Task<List<Expense>> GetByDate(DateOnly date);
}
