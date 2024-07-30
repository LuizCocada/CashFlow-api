using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;
    
    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();
        
        _mock.Setup(IPasswordEncrypter => IPasswordEncrypter.Encrypt(It.IsAny<string>()))
            .Returns("PasswordEncriptedMokado");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) == false)
        {
            _mock.Setup(IPasswordEncrypter => IPasswordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);
        }

        return this;
    }

    public IPasswordEncripter Build() => _mock.Object;

}