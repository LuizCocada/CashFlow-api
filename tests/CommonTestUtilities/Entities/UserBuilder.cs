using Bogus;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Cryptography;
using DocumentFormat.OpenXml.Wordprocessing;
using Name = Bogus.DataSets.Name;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(u => u.Password, (_, user) => passwordEncrypter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => new Guid());

        return user;
    }
}