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

public class ListDestination<TItem> : IDestination<TItem>
{
    private readonly List<TItem> _destinationData;

    public ListDestination(List<TItem> destinationData)
    {
        _destinationData = destinationData;
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _destinationData.AddRange(sourceData);
    }
}