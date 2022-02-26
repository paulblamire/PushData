using System.Linq.Expressions;

namespace PushData.Core;

public class FilterSource<TItem> : ISource<TItem>
{
    private readonly ISource<TItem> _inner;
    private readonly Expression<Func<TItem, bool>> _predicate;

    public FilterSource(ISource<TItem> inner, Expression<Func<TItem, bool>> predicate)
    {
        _inner = inner;
        _predicate = predicate;
    }
    
    public IQueryable<TItem> GetData()
    {
        return _inner.GetData().Where(_predicate);
    }
}

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