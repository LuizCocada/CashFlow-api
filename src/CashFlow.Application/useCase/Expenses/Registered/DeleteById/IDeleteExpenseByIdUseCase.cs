using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.useCase.Expenses.Registered.DeleteById;
public interface IDeleteExpenseByIdUseCase
{
    Task Execute( long id );
}
