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

    public class ItemTwo
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

        var source = new Source<Item>(sourceData);
        var destination = new Destination<Item>(destinationData);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Contains(destinationData, d => d.Id == "A" && d.Value == "A");
    }
    
    [Fact]
    public void CanSyncASingleNewItemToOneSink_ItemTwoType()
    {
        var sourceData = new List<ItemTwo>()
        {
            new ItemTwo() { Id = "A", Value = "A" }
        };
        var destinationData = new List<ItemTwo>();

        var source = new Source<ItemTwo>(sourceData);
        var destination = new Destination<ItemTwo>(destinationData);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Contains(destinationData, d => d.Id == "A" && d.Value == "A");
    }
}

public class DataSync
{
    public void Sync<T>(Source<T> source, Destination<T> destination)
    {
        var sourceData = source.GetData();
        destination.ApplyChanges(sourceData);
    }
}

public class Destination<TItem>
{
    private readonly List<TItem> _destinationData;

    public Destination(List<TItem> destinationData)
    {
        _destinationData = destinationData;
    }

    public void ApplyChanges(IEnumerable<TItem> sourceData)
    {
        _destinationData.AddRange(sourceData);
    }
}

public class Source<TItem>
{
    private readonly List<TItem> _sourceData;

    public Source(List<TItem> sourceData)
    {
        _sourceData = sourceData;
    }

    public IEnumerable<TItem> GetData()
    {
        return _sourceData;
    }
}