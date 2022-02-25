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
        var destination = new ListDestination<ItemOne, string>(destinationData, i => i.Id);
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
        var destination = new ListDestination<ItemTwo, string>(destinationData, i => i.Id);
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
        var destination = new ListDestination<ItemOne, string>(destinationData, i => i.Id);
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
        var destination = new ListDestination<ItemTwo, string>(destinationData, i => i.Id);
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
        var destination = new ListDestination<ItemTwo, string>(destinationData, i => i.Id);
        var filterDestination = new FilterDestination<ItemTwo>(destination, i => i.Id == "A");
        var mapToDestination = new MapDestination<ItemOne, ItemTwo>(filterDestination, i => new ItemTwo() { Id = i.Id, Value = i.Value + " Mapped" });
        
        var sut = new DataSync();
        sut.Sync(source, mapToDestination);

        Assert.Single(destinationData);
        Assert.Single(destinationData, d => d.Id == "A" && d.Value == "A Mapped");
    }
    
    [Fact]
    public void CanSyncASingleUpdatedItemToOneSink_ItemOneType()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A.1" }
        };
        var destinationData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A.0" }
        };

        var source = new ListSource<ItemOne>(sourceData);
        var destination = new ListDestination<ItemOne, string>(destinationData, i => i.Id);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Single(destinationData);
        Assert.Single(destinationData, d => d.Id == "A" && d.Value == "A.1");
    }
    
    [Fact]
    public void CanSyncMultipleChanges()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A.1" },
            new ItemOne() { Id = "B", Value = "B.1" },
            new ItemOne() { Id = "C", Value = "C.0" }
        };
        var destinationData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A.0" },
            new ItemOne() { Id = "B", Value = "B.0" }
        };

        var source = new ListSource<ItemOne>(sourceData);
        var destination = new ListDestination<ItemOne, string>(destinationData, i => i.Id);
        var sut = new DataSync();
        sut.Sync(source, destination);

        Assert.Equal(3,destinationData.Count);
        Assert.Single(destinationData, d => d.Id == "A" && d.Value == "A.1");
        Assert.Single(destinationData, d => d.Id == "B" && d.Value == "B.1");
        Assert.Single(destinationData, d => d.Id == "C" && d.Value == "C.0");
    }
    
    [Fact]
    public void CanSyncMultipleChangesToMultipleDestinations()
    {
        var sourceData = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A.1" },
            new ItemOne() { Id = "B", Value = "B.1" },
            new ItemOne() { Id = "C", Value = "C.0" }
        };
        var destinationDataOne = new List<ItemOne>()
        {
            new ItemOne() { Id = "A", Value = "A.0" },
            new ItemOne() { Id = "B", Value = "B.0" }
        };
        
        var destinationDataTwo = new List<ItemTwo>()
        {
            new ItemTwo() { Id = "B", Value = "B.0" },
            new ItemTwo() { Id = "C", Value = "C.0" }
        };

        var source = new ListSource<ItemOne>(sourceData);
        var destinationOne = new ListDestination<ItemOne, string>(destinationDataOne, i => i.Id);
        
        var destinationTwo = new ListDestination<ItemTwo, string>(destinationDataTwo, i => i.Id);
        var mapToDestinationTwo = new MapDestination<ItemOne, ItemTwo>(destinationTwo, i => new ItemTwo() { Id = i.Id, Value = i.Value + " Mapped" });

        var sut = new DataSync();
        sut.Sync(source, destinationOne, mapToDestinationTwo);

        Assert.Equal(3,destinationDataOne.Count);
        Assert.Single(destinationDataOne, d => d.Id == "A" && d.Value == "A.1");
        Assert.Single(destinationDataOne, d => d.Id == "B" && d.Value == "B.1");
        Assert.Single(destinationDataOne, d => d.Id == "C" && d.Value == "C.0");
        
        Assert.Equal(3,destinationDataTwo.Count);
        Assert.Single(destinationDataTwo, d => d.Id == "A" && d.Value == "A.1 Mapped");
        Assert.Single(destinationDataTwo, d => d.Id == "B" && d.Value == "B.1 Mapped");
        Assert.Single(destinationDataTwo, d => d.Id == "C" && d.Value == "C.0 Mapped");
    }
}