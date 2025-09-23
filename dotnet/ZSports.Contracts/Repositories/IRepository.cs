using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;

namespace ZSports.Contracts.Repositories;

public interface IRepository<TItem, TKey> where TItem : class
{
    Task<IDbContextTransaction> BeginTransaction();
    Task AddAsync(TItem item, CancellationToken cancellationToken = default);
    Task<TItem> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TItem>> GetPaginatedList(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    void Update(TItem item);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
