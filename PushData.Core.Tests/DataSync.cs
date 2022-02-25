namespace PushData.Core.Tests;

public class DataSync
{
    public void Sync<T>(ISource<T> source, IDestination<T> destination)
    {
        var sourceData = source.GetData();
        destination.ApplyChanges(sourceData);
    }
}