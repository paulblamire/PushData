using System.Collections.Generic;
using PushData.Core.Tests.ExampleTypes;
using Xunit;

namespace PushData.Core.Tests;

public class DataSyncTests
{
   
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