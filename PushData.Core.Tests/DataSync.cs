namespace PushData.Core.Tests;

public class DataSync
{
    public void Sync<T>(Source<T> source, Destination<T> destination)
    {
        var sourceData = source.GetData();
        destination.ApplyChanges(sourceData);
    }
}