using CashFlow.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.useCase.Expenses.Registered.Update;
public interface IUpdateExpenseUseCase
{
    public Task Execute( long id , RequestExpenseJson request );
}
