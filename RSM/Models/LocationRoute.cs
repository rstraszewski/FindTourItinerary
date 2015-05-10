using System;

namespace RSM.Models
{
    public class LocationRoute
    {
        public long Id { get; set; }
        public virtual Location LocA { get; set; }
        public virtual Location LocB { get; set; }
        public double DurationInSeconds { get; set; }

        public string DurationAsString
        {
            get
            {
                var t = TimeSpan.FromSeconds(DurationInSeconds);
                return string.Format("{0:D2} hours and {1:D2} minutes",
                    t.Hours,
                    t.Minutes);
            }
        }

        public double DurationInHours
        {
            get { return DurationInSeconds / 3600; }
        }

        public double Distance { get; set; }

    }
}