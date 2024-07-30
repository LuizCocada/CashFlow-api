using System.Text;
using CashFlow.Api.Filter;
using CashFlow.Api.Middleware;
using CashFlow.Infrastructure;
using CashFlow.Application;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = @"JWT Authorization header using the Bearer sheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 1234abcdf'",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });
    
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
           new OpenApiSecurityScheme
           {
               Reference = new OpenApiReference
               {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
               },
               Scheme = "oauth2",       
               Name = "Bearer",
               In = ParameterLocation.Header
           },
           new List<string>()
       }
    });
});

//connection string em appsettings.json

//Necess�rio para o funcionamento do filtro de exce��es.
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

//injecoes de dependencias
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAplication();

//atenticacao

var signingKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey");

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0), //acertar as horas para acertar se o token ja expirou
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!))//fazendo a msm coisa da funcao SymmetricSecurityKey Da classe JwtTokenGenerator.
    };
});
//fim

var app = builder.Build();

if( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>(); //Necess�rio para o funcionamento do CultureMiddleware

app.UseHttpsRedirection();

app.UseAuthentication();//autenticacao
app.UseAuthorization();

app.MapControllers();

if (builder.Configuration.IsTestEnvironment() == false) //para diferenciar o banco de dados em memoria ou local.
{
    await MigradeDatabase();
}

app.Run();

async Task MigradeDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();

    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}

public partial class Program { }
