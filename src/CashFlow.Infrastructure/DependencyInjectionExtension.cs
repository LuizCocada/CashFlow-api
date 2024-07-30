using CashFlow.Domain.Repositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CashFlow.Infrastructure;
//para funcionar o metodo this e em program.cs, a classe e a funcao deve ser static

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.BCrypt>(); //se chamar a Iterface encripter devolva uma instancia de Bcrypt
        AddRepositories(services);
        AddToken(services, configuration);

        if (configuration.IsTestEnvironment() == false)//se nao for um ambiente de teste executa a funcao do db local
        {
            AddDbContext(services, configuration);
        }
    }
    
    //necessita instalação do nuget Extensions.BINDER;
    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }
    
    public static void AddRepositories( IServiceCollection services )
    {
        services.AddScoped<IExpenseWriteOnlyRepository , ExpenseRepository>(); //se chamar a interface Write
        services.AddScoped<IExpenseReadOnlyRepository , ExpenseRepository>(); //se chamar a interface read
        services.AddScoped<IExpenseUpdateOnlyRepository , ExpenseRepository>(); // se chamar a interface de updade
        services.AddScoped<IUserReadOnlyRepository , UserRepository>(); 
        services.AddScoped<IUserWriteOnlyRepository , UserRepository>(); 
        
        services.AddScoped<IUnitOfWork , UnitOfWork>();

    }

    public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection"); 

        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
