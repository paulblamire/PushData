using System.Data.Entity;

namespace PushData.Core;

public class DbSetDestination<TItem, TKey> : IDestination<TItem> where TKey : notnull where TItem : class
{
    private readonly DbSet<TItem> _destinationData;
    private readonly DetectChanges<TItem, TKey> _detectChanges;
    private readonly Func<TItem, TKey> _getKey;
    private readonly DbContext _dbContext;

    public DbSetDestination(DbContext dbContext, Func<TItem, TKey> getKey)
    {
        _dbContext = dbContext;
        _destinationData = dbContext.Set<TItem>();
        _getKey = getKey;
        _detectChanges = new DetectChanges<TItem, TKey>(getKey);
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _dbContext.Configuration.AutoDetectChangesEnabled = false;
        
        _detectChanges.Process(sourceData, _destinationData,
            items => { _destinationData.AddRange(items); },
            (k, i) =>
            {
                var existingItem = _destinationData.Find(k);
                _dbContext.Entry(existingItem).CurrentValues.SetValues(i);
            });
        
        _dbContext.Configuration.AutoDetectChangesEnabled = true;
        _dbContext.SaveChanges();
    }
}