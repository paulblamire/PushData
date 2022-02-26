namespace PushData.Core;

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