using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RSM.Entities
{
    public class RsmDbContext : DbContext
    {
        public RsmDbContext() : base(Environment.MachineName)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RsmDbContext>());
        }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationRoute> LocationRoutes { get; set; }
    }
}