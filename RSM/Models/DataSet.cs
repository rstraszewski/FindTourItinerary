using System.Collections.Generic;
using System.Linq;
using System.Web;
using RSM.Controllers;

namespace RSM.Models
{
    public class DataSet
    {
        public long Id { get; set; }
        public virtual List<Location> Locations { get; set; }
        public virtual List<LocationRoute> Routes { get; set; }
    }
}