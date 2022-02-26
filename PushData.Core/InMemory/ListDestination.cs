namespace PushData.Core.InMemory;

public class ListDestination<TItem, TKey> : IDestination<TItem> where TKey : notnull
{
    private readonly List<TItem> _destinationData;
    private readonly Func<TItem, TKey> _getKey;

    public ListDestination(List<TItem> destinationData, Func<TItem, TKey> getKey)
    {
        _destinationData = destinationData;
        _getKey = getKey;
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        var dict = _destinationData.ToDictionary(_getKey, i => i);
        foreach (var item in sourceData)
        {
            var key = _getKey(item);
            if (dict.TryGetValue(key, out var existingItem))
            {
                _destinationData.Remove(existingItem);
                _destinationData.Add(item);
            }
            else
            {
                _destinationData.Add(item);
            }
        }
    }
}