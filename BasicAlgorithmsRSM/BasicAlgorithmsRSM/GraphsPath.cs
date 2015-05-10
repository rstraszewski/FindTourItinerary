using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    class GraphsPath
    {
        public List<Vertex> VerticesSequence = new List<Vertex>();

        public double SumScore()
        {
            return VerticesSequence.Sum(vertex => vertex.Score);
        }
        public void Show()
        {
            Console.WriteLine("---PATH---");
            Vertex lastVertex = null;
            double time = 0;
            foreach (var vertex in VerticesSequence)
            {
                if (lastVertex != null)
                {
                    Console.WriteLine("Edge DURATION: " + vertex.GetEdgeTo(lastVertex).Duration);
                    time += vertex.GetEdgeTo(lastVertex).Duration;
                }
                Console.WriteLine("Vertex ID: " + vertex.Id + " SCORE: " + vertex.Score + " DURATION: " + vertex.Duration);
                lastVertex = vertex;
                time += vertex.Duration;

            }
            Console.WriteLine("---SCORE: " + SumScore() + " TOTAL TIME: " + time + "---");
        }
    }
}
