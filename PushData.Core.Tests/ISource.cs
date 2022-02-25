using System.Linq;

namespace PushData.Core.Tests;

public interface ISource<out TItem>
{
    public IQueryable<TItem> GetData();
}