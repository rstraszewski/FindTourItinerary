using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    class Graph
    {
        public int NumberOfVertices { get; set; }
        public int NumberOfEdges { get; set; }

        public List<Vertex> Vertices = new List<Vertex>();
        private List<Edge> Edges = new List<Edge>();

        public Graph()
        {
            NumberOfVertices = 0;
            NumberOfEdges = 0;
        }
        public Graph(List<DistanceFromMap> listOfLocations)
        {
            NumberOfVertices = 0;
            NumberOfEdges = 0;
            var rnd = new Random();
            var size = listOfLocations.Count;
            foreach (var path in listOfLocations)
            {
                var v1 = Vertices.Any(item => item.Id == path.PositionA) ? Vertices.Find(item => item.Id == path.PositionA) : AddVertex(path.PositionA, rnd.Next(1, 100), rnd.Next(1, 10));
                var v2 = Vertices.Any(item => item.Id == path.PositionB) ? Vertices.Find(item => item.Id == path.PositionB) : AddVertex(path.PositionB, rnd.Next(1, 100), rnd.Next(1, 10));
                var e = AddEdge(v1, v2, path.Time, path.Distance);
            }
        }

        public Vertex AddVertex(int id=0, long duration=0, double score=0)
        {
            var v = new Vertex(id, duration, score);
            Vertices.Add(v);
            NumberOfVertices++;
            return v;
        }

        public Edge AddEdge(Vertex vertex1, Vertex vertex2, long duration = 0, long distance = 0)
        {
            Edge e;
            if ((e = Edges.Find(edg => edg.Vertices.Contains(vertex1) && edg.Vertices.Contains(vertex2))) != null) return e;
            vertex1.ConnectedVertices.Add(vertex2);
            vertex2.ConnectedVertices.Add(vertex1);
            e = new Edge(vertex1, vertex2, duration, distance);
            vertex1.ConnectedEdges.Add(e);
            vertex2.ConnectedEdges.Add(e);
            Edges.Add(e);
            NumberOfEdges++;
            return e;
        }

        public Edge GetEdge(Vertex vertex1, Vertex vertex2)
        {
            return Edges.Find(edge => edge.Vertices.Contains(vertex1) && edge.Vertices.Contains(vertex2));
        }

        public void TurnAllVerticesToNotVisited()
        {
            foreach (var vertex in Vertices)
            {
                vertex.Visited = false;
            }
        }

        public void Show()
        {
            Console.WriteLine("---VERTICES---");
            foreach (var vertex in Vertices)
            {
                Console.WriteLine("Vertex ID: " + vertex.Id + " SCORE: " + vertex.Score + " DURATION: " + vertex.Duration);
            }
            Console.WriteLine("---EDGES---");
            foreach (var edge in Edges)
            {
                Console.WriteLine("Edge from: " + edge.Vertices.First().Id + " to: " + edge.Vertices.Last().Id + " DURATION: " + edge.Duration);
            }
        }
    }
}
