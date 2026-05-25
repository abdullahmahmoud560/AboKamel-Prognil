using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Test;

public class ServicesContextGenerator
{
    /// <summary>
    /// Holds a singleton instance of the in-memory database context.
    /// </summary>
    private static CapsulaDbContext Context;

    /// <summary>
    /// Generates a new instance of <see cref="ParkiloDbContext"/> if not already created.
    /// Ensures the database is deleted and recreated, then populates it with test data for inventory items.
    /// </summary>
    /// <returns>An instance of <see cref="ParkiloDbContext"/> with test data.</returns>
    public static CapsulaDbContext Generator()
    {
        if (Context == null)
        {
            var options = new DbContextOptionsBuilder<CapsulaDbContext>()
               .UseInMemoryDatabase(databaseName: "ServicesDbPoc")

               .Options;

            Context = new CapsulaDbContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Context.SaveChanges();
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        return Context;
    }
}
