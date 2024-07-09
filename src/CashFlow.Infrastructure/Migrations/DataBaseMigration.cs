using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static async Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbcontext = serviceProvider.GetRequiredService<CashFlowDbContext>();

        await dbcontext.Database.MigrateAsync();
    }
}


//esta função é referenciada em program.cs

//serve para automatizar a criacao de tabelas pelo migrate sem precisar executar codigos de update no terminal.

//nao é obrigatoria.