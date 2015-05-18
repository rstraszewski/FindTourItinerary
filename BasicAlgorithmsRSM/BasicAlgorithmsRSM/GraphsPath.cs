using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    public class GraphsPath
    {
        public List<Vertex> VerticesSequence = new List<Vertex>();

        public double SumScore()
        {
            return VerticesSequence.Sum(vertex => vertex.Score);
        }

        public double Score
        {
            get
            {
                return VerticesSequence.Sum(vertex => vertex.Score);
            }
        }

        public double Duration
        {
            get
            {
                var duration = 0d;
                if (VerticesSequence.Count == 0)
                {
                    return double.MaxValue;
                }
                VerticesSequence.Aggregate((current, next) =>
                {
                    duration += GetEdgeDuration(current, next);
                    return next;
                });

                return duration;
            }
        }

        public int GetLastIndexWithinLimit(double limit)
        {
            Vertex lastVertex = null;
            var duration = 0d;
            var index = 0;
            for (var i = 0; i < VerticesSequence.Count; i++)
            {
                var vertex = VerticesSequence[i];
                var edgeDuration = GetEdgeDuration(lastVertex, vertex);
                if (duration + edgeDuration > limit)
                {
                    break;
                }
                lastVertex = vertex;
                duration += edgeDuration;
                index = i;
            }

            return index;
        }

        public double DurationWithinLimit(double limit)
        {
                var duration = 0d;
                Vertex lastVertex = null;

                for (var i = 0; i < VerticesSequence.Count; i++)
                {
                    var vertex = VerticesSequence[i];
                    var edgeDuration = GetEdgeDuration(lastVertex, vertex);
                    if (duration + edgeDuration > limit)
                    {
                        break;
                    }
                    lastVertex = vertex;
                    duration += edgeDuration;
                }

                return duration;
        }

        public double ScoreWithinLimit(double limit)
        {
            var duration = 0d;
            Vertex lastVertex = null;
            var score = 0d;

            for (var i = 0; i < VerticesSequence.Count; i++)
            {
                var vertex = VerticesSequence[i];
                var edgeDuration = GetEdgeDuration(lastVertex, vertex);

                if (duration + edgeDuration > limit)
                {
                    break;
                }

                lastVertex = vertex;
                duration += edgeDuration;
                score += vertex.Score;
            }

            return score;
        }

        public GraphsPath GetPartWithinLimit(double limit)
        {
            var path = new GraphsPath();
            var duration = 0d;
            Vertex lastVertex = null;
            for (var i = 0; i < VerticesSequence.Count; i++)
            {
                var vertex = VerticesSequence[i];
                duration += GetEdgeDuration(lastVertex, vertex);
                if (duration > limit)
                {
                    break;
                }
                path.VerticesSequence.Add(vertex);
                lastVertex = vertex;
            }

            return path;
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

        public double GetEdgeDuration(Vertex vertex1, Vertex vertex2)
        {
            if (vertex1 != null && vertex2 != null)
            {
                var edge = vertex1.ConnectedEdges.Find(e => e.Vertices.Contains(vertex1) && e.Vertices.Contains(vertex2));
                return edge.Duration;
            }

            return 0;
        }
    }
}
