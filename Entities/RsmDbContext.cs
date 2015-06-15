using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RSM.Entities
{
    public class RsmDbContext : DbContext
    {
#if DEBUG
        public RsmDbContext() : base(Environment.MachineName)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RsmDbContext>());
        }
#endif
#if RELEASE
        public RsmDbContext() : base("Default")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<RsmDbContext>());
        }
#endif
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationRoute> LocationRoutes { get; set; }
    }
}