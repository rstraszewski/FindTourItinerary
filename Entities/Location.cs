using System.Globalization;

namespace RSM.Entities
{
    public class Location
    {
        public long Id { get; set; }
        public string ForsquareId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string PhotoUrl { get; set; }

        public string LocationAsString()
        {
            return Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat) + "," + Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}