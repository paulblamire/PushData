using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace PushData.Core.Tests.TestDoubles;



public class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;
    ObservableCollection<TEntity> _data;
    IQueryable _query;

    public TestDbSet(DbContext context)
    {
        _context = context;
        _data = new ObservableCollection<TEntity>();
        _query = _data.AsQueryable();
    }

    public override TEntity Add(TEntity item)
    {
        _data.Add(item);
        _context.Entry(item).State = EntityState.Added;
        return item;
    }

    public override TEntity Remove(TEntity item)
    {
        _data.Remove(item);
        _context.Entry(item).State = EntityState.Deleted;
        return item;
    }

    public override TEntity Attach(TEntity item)
    {
        _data.Add(item);
        _context.Entry(item).State = EntityState.Unchanged;
        return item;
    }

    public override TEntity Create()
    {
        return Activator.CreateInstance<TEntity>();
    }

    public override TDerivedEntity Create<TDerivedEntity>()
    {
        return Activator.CreateInstance<TDerivedEntity>();
    }

    public override ObservableCollection<TEntity> Local
    {
        get { return _data; }
    }

    Type IQueryable.ElementType
    {
        get { return _query.ElementType; }
    }

    Expression IQueryable.Expression
    {
        get { return _query.Expression; }
    }

    IQueryProvider IQueryable.Provider
    {
        get { return new TestDbAsyncQueryProvider<TEntity>(_query.Provider); }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
    {
        return new TestDbAsyncEnumerator<TEntity>(_data.GetEnumerator());
    }

}
