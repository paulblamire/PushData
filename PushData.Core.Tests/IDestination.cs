using System.Collections.Generic;

namespace PushData.Core.Tests;

public interface IDestination<in TItem>
{
    void ApplyChanges(IEnumerable<TItem> sourceData);
}