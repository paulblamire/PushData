namespace PushData.Core;

public interface IDestination<in TItem>
{
    void ApplyChanges(IEnumerable<TItem> sourceData);
}