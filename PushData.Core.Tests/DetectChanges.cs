using System;
using System.Collections.Generic;
using System.Linq;

namespace PushData.Core.Tests;

public class DetectChanges<TItem, TKey> where TKey : notnull
{
    private readonly Func<TItem, TKey> _getKey;

    public DetectChanges(Func<TItem, TKey> getKey)
    {
        _getKey = getKey;
    }

    public void Process(IEnumerable<TItem> source, IEnumerable<TItem> destination, Action<IEnumerable<TItem>> addItems,
        Action<TKey, TItem> updateItem)
    {
        var dict = destination.ToDictionary(_getKey, i => i);
        var newItems = new List<TItem>();
        foreach (var item in source)
        {
            var key = _getKey(item);
            if (dict.ContainsKey(key))
            {
                updateItem(key, item);
            }
            else
            {
                newItems.Add(item);
            }
        }

        addItems(newItems);
    }
}