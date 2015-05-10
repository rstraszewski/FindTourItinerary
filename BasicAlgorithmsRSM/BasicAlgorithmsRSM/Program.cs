using System;
using System.Collections.Generic;

namespace BasicAlgorithmsRSM
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var graph = new Graph();
            graph.Show();
            var alg = new RepeatedNearestNeighbor(graph, 150);
            var result = alg.Performe();
            Console.WriteLine("Result:");
            result.Show();
            Console.ReadKey();
        }

    }
}
