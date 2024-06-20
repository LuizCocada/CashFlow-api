using CashFlow.Api.Filter;
using CashFlow.Api.Middleware;
using CashFlow.Infrastructure;
using CashFlow.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connection string em appsettings.json

//Necess�rio para o funcionamento do filtro de exce��es.
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

//injecoes de dependencias
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAplication();


var app = builder.Build();

if( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>(); //Necess�rio para o funcionamento do CultureMiddleware

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
