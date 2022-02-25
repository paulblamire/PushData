using System.Collections.Generic;

namespace PushData.Core.Tests;

public class Destination<TItem>
{
    private readonly List<TItem> _destinationData;

    public Destination(List<TItem> destinationData)
    {
        _destinationData = destinationData;
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _destinationData.AddRange(sourceData);
    }
}