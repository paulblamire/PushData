namespace PushData.Core.BasicTransforms;

public class FilterDestination<TItem> : IDestination<TItem>
{
    private readonly IDestination<TItem> _nextDestination;
    private readonly Func<TItem, bool> _predicate;

    public FilterDestination(IDestination<TItem> nextDestination, Func<TItem, bool> predicate)
    {
        _nextDestination = nextDestination;
        _predicate = predicate;
    }
    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        var filtered = sourceData.Where(_predicate);
        _nextDestination.ApplyChanges(filtered);
    }
}