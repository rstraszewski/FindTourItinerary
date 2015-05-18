using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    public class SimulatedAnnealing : IAlgorithm
    {
        public Graph Graph { get; private set; }
        public double TimeConstrain { get; private set; }
        public double Temperature { get; private set; }
        public double TemperatureStart { get; private set; }
        public SimulatedAnnealing(Graph graph, double timeConstrain)
        {
            this.Graph = graph;
            this.TimeConstrain = timeConstrain;
            Temperature = 100;
            TemperatureStart = Temperature;
        }

        public GraphsPath Performe()
        {
            return Performe(1000);
        }

        public GraphsPath Result { get; set; }

        public GraphsPath Performe(int numberOfIteration)
        {
            var alfa = 20;
            var rnd = new Random();
            Result = GetWholePathRandom();

            for (var iter = 0; iter < numberOfIteration; iter++)
            {
                var randomDouble = rnd.NextDouble();
                var numberToShuffle = (Temperature / TemperatureStart) *
                                      Graph.NumberOfVertices * (0.5);

                var path = GetNeigbourWholePath(Math.Ceiling(numberToShuffle));
                var p = Math.Pow(Math.E, -TemperatureStart * (Result.ScoreWithinLimit(TimeConstrain) - path.ScoreWithinLimit(TimeConstrain)) / (Temperature));//rozklad normalny, peak przy 1

                if (randomDouble < p)
                {
                    Result = path;
                }

                Temperature = 0.99 * Temperature;
            }
            return Result.GetPartWithinLimit(TimeConstrain);
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
                var index = path.GetLastIndexWithinLimit(TimeConstrain);

                var i = randomIndexes.First(r => r < index);

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

    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
