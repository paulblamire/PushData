using System.Linq.Expressions;

namespace PushData.Core.BasicTransforms;

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