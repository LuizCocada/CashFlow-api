using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityResponse(); 
    }

    private void RequestToEntity()
    {
        CreateMap<RequestExpenseJson , Expense>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(destino => destino.Password,
                config => config.Ignore());
    }

    private void EntityResponse()
    {
        CreateMap<Expense , ResponseRegisteredExpenseJson>();
        CreateMap<Expense , ResponseShortExpenseJson>();
        CreateMap<Expense , ResponseExpenseByIdJson>();
    }
}

//IGNORAR PASSWORD
//.ForMember(destino => destino.Password,
// config => config.Ignore());
