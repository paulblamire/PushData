using System.Data.Entity;
using System.Linq;

namespace PushData.Core.Tests;

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