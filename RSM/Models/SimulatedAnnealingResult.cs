using System.Collections.Generic;
using System.Linq;
using BasicAlgorithmsRSM;
using RSM.Entities;

namespace RSM.Models
{
    public class SimulatedAnnealingResult
    {
        public SimulatedAnnealingResult()
        {
            Paths = new List<List<Location>>();
            Scores = new List<double>();
        }

        public SimulatedAnnealingParameters Parameters { get; set; }
        public List<List<Location>> Paths { get; set; }
        public List<double> Scores { get; set; }

        public double ScoreAvarege
        {
            get { return Scores.Sum()/Scores.Count; }
        }

        public DataSetViewModel DataSet { get; set; }
        public int Id { get; set; }
    }
}