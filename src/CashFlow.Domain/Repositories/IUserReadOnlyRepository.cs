using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);

    Task<User?> GetUserByEmail(string email);
}