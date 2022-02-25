using System.Collections.Generic;
using PushData.Core.Tests.ExampleTypes;
using Xunit;

namespace PushData.Core.Tests;

public class DataSyncTests
{
   
    [Fact]
    public void CanSyncASingleNewItemToOneSink_ItemOneType()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A" }
        };
        var destinationData = new List<ItemOne>();

        var source = new Source<ItemOne>(sourceData);
        var destination = new Destination<ItemOne>(destinationData);
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