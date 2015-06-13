namespace RSM.Models
{
    public class AntColonyTask
    {
        public DataSetViewModel DataSet { get; set; }
        public double TimeConstrain { get; set; }
        public double NumberOfIterations { get; set; }
        public double NumberOfAntsPerVertex { get; set; }
        public double EvaporationCoefficient { get; set; }
        public double IntensityDerivative { get; set; }
        public double InitialPheromonValue { get; set; }
        public double TrailImportance { get; set; }
        public double VisibilityImportance { get; set; }
        public int Id { get; set; }
    }
}