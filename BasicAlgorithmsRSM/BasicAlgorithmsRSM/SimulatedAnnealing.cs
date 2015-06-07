using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    public class SimulatedAnnealingParameters
    {
        public int NumberOfIteration { get; set; }
        public double TemperatureAlpha { get; set; }
        public double TemperatureMax { get; set; }
        public double TimeConstrain { get; set; }

    }
    public class SimulatedAnnealing : IAlgorithm
    {
        public Graph Graph { get; private set; }
        public double Temperature { get; private set; }
        public SimulatedAnnealingParameters Parameters { get; private set; }
        public SimulatedAnnealing(Graph graph, SimulatedAnnealingParameters parameters)
        {
            Graph = graph;
            Temperature = parameters.TemperatureMax;
            Parameters = parameters;
        }

        public GraphsPath Result { get; set; }

        public GraphsPath Performe()
        {
            var rnd = new Random();
            Result = GetWholePathRandom();

            for (var iter = 0; iter < Parameters.NumberOfIteration; iter++)
            {
                var randomDouble = rnd.NextDouble();
                var numberToShuffle = (Temperature / Parameters.TemperatureMax) *
                                      Graph.NumberOfVertices * (0.5);

                var path = GetNeigbourWholePath(Math.Ceiling(numberToShuffle));
                var p = Math.Pow(Math.E, -Parameters.TemperatureMax * (Result.ScoreWithinLimit(Parameters.TimeConstrain) - path.ScoreWithinLimit(Parameters.TimeConstrain)) / (Temperature));//rozklad normalny, peak przy 1

                if (randomDouble < p)
                {
                    Result = path;
                }

                Temperature = Parameters.TemperatureAlpha * Temperature;
            }
            return Result.GetPartWithinLimit(Parameters.TimeConstrain);
        }

        private GraphsPath GetWholePathRandom()
        {
            var randomIndexes = Enumerable.Range(0, Graph.NumberOfVertices).ToList();
            randomIndexes.Shuffle();

            var path = new GraphsPath();

            for (var i = 0; i < randomIndexes.Count; i++)
            {
                var vertex = Graph.Vertices[randomIndexes[i]];
                path.VerticesSequence.Add(vertex);
            }

            return path;
        }

        private GraphsPath GetNeigbourWholePath(double howMany)
        {
            var randomIndexes = Enumerable.Range(0, Graph.NumberOfVertices).ToList();
            randomIndexes.Shuffle();

            var path = new GraphsPath();
            path.VerticesSequence.AddRange(Result.VerticesSequence);


            if (howMany == 1)
            {
                var index = path.GetLastIndexWithinLimit(Parameters.TimeConstrain);

                var i = randomIndexes.First(r => r <= index);

                var j = randomIndexes.First(r => r != i);

                var temp = path.VerticesSequence[randomIndexes[i]];
                path.VerticesSequence[randomIndexes[i]] = path.VerticesSequence[randomIndexes[j]];
                path.VerticesSequence[randomIndexes[j]] = temp;
            }
            else
            {
                for (var i = 0; i < howMany * 2; i += 2)
                {
                    var temp = path.VerticesSequence[randomIndexes[i]];
                    path.VerticesSequence[randomIndexes[i]] = path.VerticesSequence[randomIndexes[i + 1]];
                    path.VerticesSequence[randomIndexes[i + 1]] = temp;
                }
            }
           
            return path;
        }

    }
}
