using System.Collections.Generic;
using BasicAlgorithmsRSM;
using RSM.Entities;

namespace RSM.Models
{
    public class RepeatedNearestNeighborResult
    {
        public RepeatedNearestNeighborParameters Parameters { get; set; }
        public List<Location> Path { get; set; }
        public double Score { get; set; }
        public DataSetViewModel DataSet { get; set; }
        public int Id { get; set; }
    }
}