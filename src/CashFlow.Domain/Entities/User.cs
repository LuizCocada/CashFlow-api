using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; }
    public string Role { get; set; } = Roles.TEAM_MEMBER;
}

//Role esta fixa como TEAM_MEMBER, pois, estou considerando que so pessoas com niveis mais baixos poderao irao registrar as contas(expenses).