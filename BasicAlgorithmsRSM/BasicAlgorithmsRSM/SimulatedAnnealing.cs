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
        public GraphsPath Result { get; private set; }
        public int Temperature { get; private set; }
        public SimulatedAnnealing(Graph graph, double timeConstrain)
        {
            this.Graph = graph;
            this.TimeConstrain = timeConstrain;
            this.Result = new GraphsPath();
            Temperature = 100;
        }

        public GraphsPath Performe()
        {
            return Performe(Temperature);
        }

        public GraphsPath Performe(int numberOfIteration)
        {
            Random rnd = new Random();
            for (var iter = 0; iter < numberOfIteration; iter++)
            {
                Temperature--;
                var randomDouble = rnd.NextDouble();

                var path = GetRandomSequance();
                if (path.SumScore() > Result.SumScore())
                {
                    Result = path;
                }
            }
            return Result;
        }

        private GraphsPath GetRandomSequance()
        {
            Vertex lastVertex = null;
            var randomIndexes = Enumerable.Range(0, Graph.NumberOfVertices).ToList();
            randomIndexes.Shuffle();

            var duration = 0d;
            var path = new GraphsPath();

            foreach (var i in randomIndexes)
            {
                var vertex = Graph.Vertices[i];
                
                duration += Graph.GetEdgeDuration(lastVertex, vertex);
                lastVertex = vertex;

                if (duration >= TimeConstrain)
                {
                    return path;
                }

                path.VerticesSequence.Add(vertex);
            }

            return path;
        }

        private GraphsPath GetNeigbouringSequance()
        {
            

            //var duration = 0d;
            var path = new GraphsPath();
            path.VerticesSequence.AddRange(Result.VerticesSequence.ToList());

            
            var randomIndexes = Enumerable.Range(0, Graph.NumberOfVertices).ToList();
            randomIndexes.Shuffle();

            var vertex = Graph.Vertices[randomIndexes.First()];

            if (!path.VerticesSequence.Contains(vertex))
            {
                path.VerticesSequence[0] = vertex;
            }

            vertex = Graph.Vertices[randomIndexes[1]];

            if (!path.VerticesSequence.Contains(vertex))
            {
                path.VerticesSequence[path.VerticesSequence.Count - 1] = vertex;
            }

            if (path.Duration > TimeConstrain)
            {
                path.VerticesSequence.Clear();
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
