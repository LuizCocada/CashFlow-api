using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;


namespace WebApi.Tests.Users.Register;

public class RegisterUserTest : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly string METHOD = "api/User";
    
    public RegisterUserTest(CustomWebApiFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();//instancia preparada da classe HttpClient
    }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var result = await _httpClient.PostAsJsonAsync(METHOD,request);
        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();//lendo a resposta como Stream.

        var response = await JsonDocument.ParseAsync(body);//resposta da api em um Objeto JSON

        response.RootElement.GetProperty("name").GetString().Should()
            .Be(request.Name); //verificando se no objeto Json existe a propriedade name
        
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
}