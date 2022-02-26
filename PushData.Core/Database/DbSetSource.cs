using System.Data.Entity;

namespace PushData.Core.Database;

public class DbSetSource<TItem> : ISource<TItem> where TItem : class
{
    private readonly DbContext _dbContext;

    public DbSetSource(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<TItem> GetData()
    {
        return _dbContext.Set<TItem>();
    }
}