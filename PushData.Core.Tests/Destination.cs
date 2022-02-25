using System.Collections.Generic;
using System.Linq;

namespace PushData.Core.Tests;

public interface IDestination<in TItem>
{
    void ApplyChanges(IEnumerable<TItem> sourceData);
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