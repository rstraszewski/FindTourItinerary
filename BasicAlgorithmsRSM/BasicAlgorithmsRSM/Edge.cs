using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    class Edge
    {
        public long Distance { get; set; }
        public long Duration { get; set; }
        public double PheromonIntensity { get; set; }

        public List<Vertex> Vertices;

        public Edge()
        {
            Vertices = new List<Vertex>();
        }

        public Edge(Vertex vertex1, Vertex vertex2, long duration=0, long distance=0)
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
    }
}
