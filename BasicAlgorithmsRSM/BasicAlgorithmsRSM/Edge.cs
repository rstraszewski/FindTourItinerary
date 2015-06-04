using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    public class Edge
    {
        public double Distance { get; set; }
        public double Duration { get; set; }
        public double PheromonIntensity { get; set; }

        public List<Vertex> Vertices;

        public Edge()
        {
            Vertices = new List<Vertex>();
        }

        public Edge(Vertex vertex1, Vertex vertex2, double duration = 0, double distance = 0)
        {
            Vertices = new List<Vertex>();

            if (vertex1 != null)
                Vertices.Add(vertex1);
            else
                throw new ArgumentNullException("vertex1");

            if (vertex2 != null)
                 Vertices.Add(vertex2);
            else
                throw new ArgumentNullException("vertex2");

            this.Duration = duration;
            this.Distance = distance;
        }

        public Vertex GetVertexOppositTo(Vertex vertex)
        {
            return Vertices.Find(v => !v.Equals(vertex));
        }
    }
}
