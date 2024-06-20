using CashFlow.Domain.Repositories;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CashFlow.Infrastructure;
//para funcionar o metodo this e em program.cs, a classe e a funcao deve ser static

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
    }



    public static void AddRepositories( IServiceCollection services )
    {
        services.AddScoped<IExpenseWriteOnlyRepository , ExpenseRepository>(); //se chamar a interface Write
        services.AddScoped<IExpenseReadOnlyRepository , ExpenseRepository>(); //se chamar a interface read
        services.AddScoped<IUnitOfWork , UnitOfWork>();

    }

    public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection"); 

        var version = new Version(8 , 0 , 37);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

}
