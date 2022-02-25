using System;
using System.Collections.Generic;
using System.Linq;

namespace PushData.Core.Tests;

public interface IDestination<in TItem>
{
    void ApplyChanges(IEnumerable<TItem> sourceData);
}

public class MapDestination<TItemIn, TItemOut> : IDestination<TItemIn>
{
    private readonly IDestination<TItemOut> _nextDestination;
    private readonly Func<TItemIn, TItemOut> _mappingFunction;

    public MapDestination(IDestination<TItemOut> nextDestination, Func<TItemIn, TItemOut> mappingFunction)
    {
        _nextDestination = nextDestination;
        _mappingFunction = mappingFunction;
    }

    public void ApplyChanges(IEnumerable<TItemIn> sourceData)
    {
        var mapped = sourceData.Select(_mappingFunction);
        _nextDestination.ApplyChanges(mapped);
    }
}

public class ListDestination<TItem, TKey> : IDestination<TItem> where TKey : notnull
{
    private readonly List<TItem> _destinationData;
    private readonly Func<TItem, TKey> _getKey;
    private readonly DetectChanges<TItem, TKey> _detectChanges;

    public ListDestination(List<TItem> destinationData, Func<TItem, TKey> getKey)
    {
        _destinationData = destinationData;
        _getKey = getKey;
        _detectChanges = new DetectChanges<TItem, TKey>(getKey);
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _detectChanges.Process(sourceData, _destinationData,
            items =>
            {
                _destinationData.AddRange(items);
            },
            (k, i) =>
            {
                _destinationData.RemoveAll(u => Equals(k, _getKey(u)));
                _destinationData.Add(i);
            });
    }
}

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