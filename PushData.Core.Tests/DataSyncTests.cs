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

        var source = new ListSource<ItemOne>(sourceData);
        var destination = new ListDestination<ItemOne>(destinationData);
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

        var source = new ListSource<ItemTwo>(sourceData);
        var destination = new ListDestination<ItemTwo>(destinationData);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Contains(destinationData, d => d.Id == "A" && d.Value == "A");
    }
    
    [Fact]
    public void CanFilterSourceItems()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A" },
            new ItemOne() { Id = "B", Value = "B" }

        };
        var destinationData = new List<ItemOne>();

        var innerSource =  new ListSource<ItemOne>(sourceData);
        var source = new FilterSource<ItemOne>(innerSource, i => i.Id == "A");
        var destination = new ListDestination<ItemOne>(destinationData);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Single(destinationData);
        Assert.Single(destinationData, d => d.Id == "A" && d.Value == "A");
    }
    
    [Fact]
    public void CanMapToADifferentDestinationType()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A" },
        };
        var destinationData = new List<ItemTwo>();

        var source =  new ListSource<ItemOne>(sourceData);
        var destination = new ListDestination<ItemTwo>(destinationData);
        var mapToDestination = new MapDestination<ItemOne, ItemTwo>(destination, i => new ItemTwo() { Id = i.Id, Value = i.Value + " Mapped" });
        var sut = new DataSync();
        sut.Sync(source, mapToDestination);
        
        Assert.Single(destinationData, d => d.Id == "A" && d.Value == "A Mapped");
    }
    
    [Fact]
    public void CanFilterDestinationItems()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A" },
            new ItemOne() { Id = "B", Value = "B" },
        };
        var destinationData = new List<ItemTwo>();

        var source =  new ListSource<ItemOne>(sourceData);
        var destination = new ListDestination<ItemTwo>(destinationData);
        var filterDestination = new FilterDestination<ItemTwo>(destination, i => i.Id == "A");
        var mapToDestination = new MapDestination<ItemOne, ItemTwo>(filterDestination, i => new ItemTwo() { Id = i.Id, Value = i.Value + " Mapped" });
        
        var sut = new DataSync();
        sut.Sync(source, mapToDestination);

        Assert.Single(destinationData);
        Assert.Single(destinationData, d => d.Id == "A" && d.Value == "A Mapped");
    }
}