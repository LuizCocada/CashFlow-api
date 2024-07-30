using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}