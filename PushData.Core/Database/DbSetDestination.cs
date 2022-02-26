﻿using System.Data.Entity;

namespace PushData.Core.Database;

public class DbSetDestination<TItem> : IDestination<TItem> where TItem : class
{
    private readonly DbSet<TItem> _destinationData;
    private readonly Func<TItem, object[]> _getKey;
    private readonly DbContext _dbContext;

    public DbSetDestination(DbContext dbContext, Func<TItem, object[]> getKey)
    {
        _dbContext = dbContext;
        _destinationData = dbContext.Set<TItem>();
        _getKey = getKey;
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _dbContext.Configuration.AutoDetectChangesEnabled = false;

        foreach (var item in sourceData)
        {
            var key = _getKey(item);
            var existingItem = _destinationData.Find(key);
            if (existingItem == null)
            {
                _destinationData.Add(item);
            }
            else
            {
                _dbContext.Entry(existingItem).CurrentValues.SetValues(item);
            }
        }
        
        _dbContext.Configuration.AutoDetectChangesEnabled = true;
        _dbContext.SaveChanges();
    }
}