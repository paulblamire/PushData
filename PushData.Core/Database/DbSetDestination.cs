using System.Data.Entity;

namespace PushData.Core.Database;

public class DbSetDestination<TItem, TKey> : IDestination<TItem> where TItem : class where TKey : notnull
{
    private readonly DbSet<TItem> _destinationData;
    private readonly Func<TItem, TKey> _getKey;
    private readonly DbContext _dbContext;

    public DbSetDestination(DbContext dbContext, DbSet<TItem> dbSet, Func<TItem, TKey> getKey)
    {
        _dbContext = dbContext;
        _destinationData = dbSet;
        _getKey = getKey;
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _dbContext.Configuration.AutoDetectChangesEnabled = false;

        var dict = _destinationData.Local.ToDictionary(_getKey, i => i);
        
        foreach (var item in sourceData)
        {
            var key = _getKey(item);
            
            if (dict.TryGetValue(key, out var existingItem))
            {
                _dbContext.Entry(existingItem).CurrentValues.SetValues(item);
            }
            else
            {
                _destinationData.Add(item);
            }
        }
        
        _dbContext.Configuration.AutoDetectChangesEnabled = true;
        _dbContext.SaveChanges();
    }
}