using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public CashFlowDbContext( DbContextOptions options ) : base(options) { }//está passando as opcoes do dbContextOptions para a classe base(DbContext)
    
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
}


//Nome da TABELA Ex: Expenses; => precisa ser o mesmo nome da tabela no Banco.