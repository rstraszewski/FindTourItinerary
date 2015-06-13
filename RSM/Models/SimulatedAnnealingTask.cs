namespace RSM.Models
{
    public class SimulatedAnnealingTask
    {
        public DataSetViewModel DataSet { get; set; }
        public int NumberOfIteration { get; set; }
        public float TemperatureAlpha { get; set; }
        public double TemperatureMax { get; set; }
        public double TimeConstrain { get; set; }
        public int HomManyTimes { get; set; }
        public int Id { get; set; }
    }
}