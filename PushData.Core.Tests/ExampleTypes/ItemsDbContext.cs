using System.Data.Entity;
using PushData.Core.Tests.TestDoubles;

namespace PushData.Core.Tests.ExampleTypes;

public class ItemsDbContext : DbContext
{
    public ItemsDbContext()
    {
        System.Data.Entity.Database.SetInitializer<ItemsDbContext>(
            new NullDatabaseInitializer<ItemsDbContext>());
        
        Items = new TestDbSet<ItemOne>(this);
    }

    public DbSet<ItemOne> Items { get; set; }

    public override int SaveChanges()
    {
        return 0;
    }
}