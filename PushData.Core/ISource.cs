namespace PushData.Core;

public interface ISource<out TItem>
{
    public IQueryable<TItem> GetData();
}