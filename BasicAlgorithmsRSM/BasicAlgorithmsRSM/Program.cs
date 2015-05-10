using System;
using System.Collections.Generic;

namespace BasicAlgorithmsRSM
{
    class Program
    {
        public static List<DistanceFromMap> FullfillGraph()
        {
            var rnd = new Random();
            var exampleList = new List<DistanceFromMap>
            {
                new DistanceFromMap()
                {
                    Distance = rnd.Next(1, 100),
                    PositionA = 0,
                    PositionB = 1,
                    Time = rnd.Next(1, 50)
                },
                new DistanceFromMap()
                {
                    Distance = rnd.Next(1, 100),
                    PositionA = 0,
                    PositionB = 2,
                    Time = rnd.Next(1, 50)
                },
                new DistanceFromMap()
                {
                    Distance = rnd.Next(1, 100),
                    PositionA = 0,
                    PositionB = 3,
                    Time = rnd.Next(1, 50)
                },
                new DistanceFromMap()
                {
                    Distance = rnd.Next(1, 100),
                    PositionA = 1,
                    PositionB = 2,
                    Time = rnd.Next(1, 50)
                },
                new DistanceFromMap()
                {
                    Distance = rnd.Next(1, 100),
                    PositionA = 1,
                    PositionB = 3,
                    Time = rnd.Next(1, 50)
                },
                new DistanceFromMap()
                {
                    Distance = rnd.Next(1, 100),
                    PositionA = 3,
                    PositionB = 2,
                    Time = rnd.Next(1, 50)
                }
            };
            ;
            return exampleList;
        }
        static void Main(string[] args)
        {
            var graph = new Graph(FullfillGraph());
            graph.Show();
            var alg = new RepeatedNearestNeighbor(graph, 150);
            var result = alg.Performe();
            Console.WriteLine("Result:");
            result.Show();
            Console.ReadKey();
        }

    }
}
