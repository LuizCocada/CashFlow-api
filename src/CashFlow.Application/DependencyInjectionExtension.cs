using CashFlow.Application.AutoMapper;
using CashFlow.Application.useCase.Expenses.Register;
using CashFlow.Application.useCase.Expenses.Registered.DeleteById;
using CashFlow.Application.useCase.Expenses.Registered.GetAll;
using CashFlow.Application.useCase.Expenses.Registered.GetById;
using CashFlow.Application.useCase.Expenses.Registered.Update;
using CashFlow.Application.useCase.Expenses.Reports.Excell;
using CashFlow.Application.useCase.Expenses.Reports.Pdf;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddAplication( this IServiceCollection services )
    {
        AddUseCases(services);
        AddAutoMapper(services);
    }

    private static void AddAutoMapper( this IServiceCollection services )
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases( this IServiceCollection services )
    {
        services.AddScoped<IRegisterExpenseUseCase , RegisterExpensesUseCase>();
        services.AddScoped<IGetAllExpenseUseCase , GetAllExpenseUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase , GetExpenseByIdUseCase>();
        services.AddScoped<IDeleteExpenseByIdUseCase , DeleteExpenseByIdUseCase>();
        services.AddScoped<IUpdateExpenseUseCase , UpdateExpenseUseCase>();
        services.AddScoped<IGereneteExpenseReportExcelUseCase , GereneteExpenseReportExcelUseCase>();
        services.AddScoped<IGereneteExpenseReportPdfUseCase, GereneteExpenseReportPdfUseCase>();
    }
}

