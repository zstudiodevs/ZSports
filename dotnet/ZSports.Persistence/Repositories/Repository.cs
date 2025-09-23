using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;
using ZSports.Contracts.Repositories;

namespace ZSports.Persistence.Repositories;

public class Repository<TItem, TKey>(ZSportsDbContext dbContext) : IRepository<TItem, TKey> where TItem : class
{
    public async Task AddAsync(TItem item, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<TItem>().AddAsync(item, cancellationToken);
    }

    public async Task<TItem> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        TItem? item = await dbContext.Set<TItem>().FindAsync(id, cancellationToken);

        if (item is null)
            throw new KeyNotFoundException($"Item with id {id} not found.");

        return item;
    }

    public async Task<IEnumerable<TItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TItem>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TItem>> GetPaginatedList(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TItem>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public void Update(TItem item)
    {
        dbContext.Set<TItem>().Update(item);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransaction()
    {
        return dbContext.Database.BeginTransaction();
    }
}
