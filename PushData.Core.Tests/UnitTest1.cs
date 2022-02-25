using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace PushData.Core.Tests;

public class DataSyncTests
{
    public class Item
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
    
    [Fact]
    public void CanSyncASingleNewItemToOneSink()
    {
        var sourceData = new List<Item>()
        {
            new Item() { Id = "A", Value = "A" }
        };
        var destinationData = new List<Item>();
        
        var source = new Source(sourceData);
        var destination = new Destination(destinationData);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Contains(destinationData, d => d.Id == "A" && d.Value == "A");
    }
}

public class DataSync
{
    public void Sync(Source source, Destination destination)
    {
        throw new System.NotImplementedException();
    }
}

public class Destination
{
    public Destination(List<DataSyncTests.Item> destinationData)
    {
        throw new System.NotImplementedException();
    }
}

public class Source
{
    public Source(List<DataSyncTests.Item> sourceData)
    {
        throw new System.NotImplementedException();
    }
}