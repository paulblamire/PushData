namespace PushData.Core;

public class DataSync
{
    public void Sync<T>(ISource<T> source, params IDestination<T>[] destinations)
    {
        var sourceData = source.GetData().ToList();
        foreach (var destination in destinations)
        {
            destination.ApplyChanges(sourceData);
        }
    }
}