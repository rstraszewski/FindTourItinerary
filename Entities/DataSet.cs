using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSM.Entities
{
    public class DataSet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public virtual List<Location> Locations { get; set; }
        public virtual List<LocationRoute> Routes { get; set; }
    }
}