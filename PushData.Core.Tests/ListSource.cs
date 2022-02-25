using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PushData.Core.Tests;

public interface ISource<out TItem>
{
    public IQueryable<TItem> GetData();
}

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

public class ListSource<TItem> : ISource<TItem>
{
    private readonly IQueryable<TItem> _sourceData;

    public ListSource(List<TItem> sourceData)
    {
        _sourceData = sourceData.AsQueryable();
    }

    public IQueryable<TItem> GetData()
    {
        return _sourceData;
    }
}