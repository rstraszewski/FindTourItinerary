using System.Collections.Generic;
using System.Linq;

namespace RSM.Models
{
    public class Result
    {
        public Result()
        {
            SimulatedAnnealingResults = new List<SimulatedAnnealingResult>();
            AntColonyResults = new List<AntColonyResult>();
            RepeatedNearestNeighborResults = new List<RepeatedNearestNeighborResult>();
        }

        public List<SimulatedAnnealingResult> SimulatedAnnealingResults { get; set; }
        public List<AntColonyResult> AntColonyResults { get; set; }
        public List<RepeatedNearestNeighborResult> RepeatedNearestNeighborResults { get; set; }

        public double AverageSaScore
        {
            get
            {
                return SimulatedAnnealingResults.Count > 0
                    ? SimulatedAnnealingResults.Sum(e => e.ScoreAvarege)/SimulatedAnnealingResults.Count
                    : 0;
            }
        }

        public double AverageNnScore
        {
            get
            {
                return RepeatedNearestNeighborResults.Count > 0
                    ? RepeatedNearestNeighborResults.Sum(e => e.Score)/RepeatedNearestNeighborResults.Count
                    : 0;
            }
        }

        public double AverageAntScore
        {
            get { return AntColonyResults.Count > 0 ? AntColonyResults.Sum(e => e.ScoreAvarege) / AntColonyResults.Count : 0; }
        }
    }
}