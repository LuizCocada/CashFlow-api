using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");//se ela nao encontrar isso em Test.Json
                                                            //ela retorna false; assim diferenciando o ambiente de test
                                                            //e o embiente de desenvolvimento
    }
}