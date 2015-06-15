using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
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
                //var randomDouble = rnd.NextDouble();
                var u1 = rnd.NextDouble(); //these are uniform(0,1) random doubles
                var u2 = rnd.NextDouble();
                var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                             Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                var randNormal =
                             1 + 0.2 * randStdNormal; //random normal(mean,stdDev^2)
                var numberToShuffle = (Temperature / Parameters.TemperatureMax) *
                                      Graph.NumberOfVertices * (0.5);

                var path = GetNeigbourWholePath(Math.Ceiling(numberToShuffle));
                var delta = Result.ScoreWithinLimit(Parameters.TimeConstrain) -
                            path.ScoreWithinLimit(Parameters.TimeConstrain);
                //var delta2 = Math.Pow(Result.ScoreWithinLimit(Parameters.TimeConstrain),3)/Result.DurationWithinLimit(Parameters.TimeConstrain) - Math.Pow(path.ScoreWithinLimit(Parameters.TimeConstrain),3) / path.DurationWithinLimit(Parameters.TimeConstrain);


                var p = Math.Pow(Math.E, -Parameters.TemperatureMax * (delta) / (5*Temperature));//rozklad normalny, peak przy 1

                if (delta < 0 || randNormal < p)
                {
                    Result = path;
                }

                //Temperature = Parameters.TemperatureAlpha * Temperature;
                Temperature = Parameters.TemperatureMax * Math.Pow(Parameters.TemperatureAlpha, iter);
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

                var temp = path.VerticesSequence[i];
                path.VerticesSequence[i] = path.VerticesSequence[j];
                path.VerticesSequence[j] = temp;
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
