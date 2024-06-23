using CashFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Domain.Repositories;
public interface IExpenseWriteOnlyRepository
{
    Task add( Expense expense );

    Task<bool> delete( long id);
}
