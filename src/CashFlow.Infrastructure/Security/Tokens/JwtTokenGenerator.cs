using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Infrastructure.Security.Tokens;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;
    
    public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)//recebendo da injecao de dependencia.
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signingKey = signingKey;
    }

    public string Generate(User user)
    {

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandle = new JwtSecurityTokenHandler();
        var securityToken = tokenHandle.CreateToken(tokenDescriptor);

        return tokenHandle.WriteToken(securityToken); //devolmento em string;
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey); //obtendo os bytes da string;
        
        return new SymmetricSecurityKey(key);
    }
}


//var claims = new List<Claim>()
// {
//     new Claim(ClaimTypes.Name, user.Name),
//     new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString())
// };

//AQUI APENAS O USERIDENTIFIER É NECESSÁRIO PARA A API IDENTIFICAR QUEM ESTÁ ACESSANDO/
//OBS, NAO PASSAR DADOS SIGILOSOS PELO CLAIM, POIS ELES NAO SAO CRIPTOGRAFADOS