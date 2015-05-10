using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    public class Vertex
    {
        public long Id { get; set; }
        public double Score { get; set; }
        public long Duration { get; set; }
        public bool Visited { get; set; }
        public List<Vertex> ConnectedVertices = new List<Vertex>();
        public List<Edge> ConnectedEdges = new List<Edge>(); 

        public Vertex(long id=0, long duration=0, double score=0)
        {
            this.Id = id;
            this.Duration = duration;
            this.Visited = false;
            this.Score = score;
        }

        public Vertex GetOppositVertexFrom(Edge edge)
        {
            return ConnectedEdges.Any(e => e.Equals(edge)) 
                ? edge.Vertices.Find(v => !v.Equals(this)) 
                : null;
        }

        public Edge GetEdgeTo(Vertex vertex)
        {
            return ConnectedVertices.Any(v => v.Equals(vertex))
                ? ConnectedEdges.Find(e => e.Vertices.Contains(vertex))
                : null;
        }
    }
}
