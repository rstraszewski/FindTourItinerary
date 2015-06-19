using System.Collections.Generic;
using System.Linq;

namespace RSM.Models
{
    public class DataSetResult
    {
        public string DataSet { get; set; }
        public List<double> ScoresSa { get; set; }

        public double ScoresSaAvarage
        {
            get { return ScoresSa.Count == 0 ? 0 : ScoresSa.Sum() / ScoresSa.Count; }
        }
        public double ScoresNnAvarage
        {
            get { return ScoresNn.Count == 0 ? 0 : ScoresNn.Sum() / ScoresNn.Count; }
        }
        public double ScoresAntAvarage
        {
            get { return ScoresAnt.Count == 0 ? 0 : ScoresAnt.Sum() / ScoresAnt.Count; }
        }
        public List<double> ScoresNn { get; set; }
        public List<double> ScoresAnt { get; set; }

        public DataSetResult()
        {
            ScoresSa = new List<double>();
            ScoresAnt = new List<double>();
            ScoresNn = new List<double>();
        }

    }
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

        public List<DataSetResult> DataSetResults
        {
            get
            {
                var list = new List<DataSetResult>();
                
                foreach (var sim in SimulatedAnnealingResults)
                {
                    var elemList = list.FirstOrDefault(elem => elem.DataSet == sim.DataSet.Name);
                    if (elemList != null)
                    {
                        elemList.ScoresSa.Add(sim.ScoreAvarege);
                    }
                    else
                    {
                        list.Add(new DataSetResult()
                        {
                            DataSet = sim.DataSet.Name,
                            ScoresSa = new List<double>(){sim.ScoreAvarege}
                        });
                    }
                }

                foreach (var sim in AntColonyResults)
                {
                    var elemList = list.FirstOrDefault(elem => elem.DataSet == sim.DataSet.Name);
                    if (elemList != null)
                    {
                        elemList.ScoresAnt.Add(sim.ScoreAvarege);
                    }
                    else
                    {
                        list.Add(new DataSetResult()
                        {
                            DataSet = sim.DataSet.Name,
                            ScoresAnt = new List<double>(){sim.ScoreAvarege}
                        });
                    }
                }
                foreach (var sim in RepeatedNearestNeighborResults)
                {
                    var elemList = list.FirstOrDefault(elem => elem.DataSet == sim.DataSet.Name);
                    if (elemList != null)
                    {
                        elemList.ScoresNn.Add(sim.Score);
                    }
                    else
                    {
                        list.Add(new DataSetResult()
                        {
                            DataSet = sim.DataSet.Name,
                            ScoresNn = new List<double>() { sim.Score }
                        });
                    }
                }
                return list;
            }
        }

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