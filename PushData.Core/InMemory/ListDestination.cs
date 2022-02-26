namespace PushData.Core.InMemory;

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