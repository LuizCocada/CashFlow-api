using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Login;


public class LoginBuilder
{
    public static RequestLogin Build()
    {
        return new Faker<RequestLogin>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}