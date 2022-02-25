using System.Collections.Generic;

namespace PushData.Core.Tests;

public class Source<TItem>
{
    private readonly List<TItem> _sourceData;

    public Source(List<TItem> sourceData)
    {
        _sourceData = sourceData;
    }

    public IEnumerable<TItem> GetData()
    {
        return _sourceData;
    }
}