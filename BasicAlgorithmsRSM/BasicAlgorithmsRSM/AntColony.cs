using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    class Ant
    {
        public Vertex CurrentPosition;
        public GraphsPath TraveledDistance;

        public Ant(Vertex currentPosition)
        {
            CurrentPosition = currentPosition;
            TraveledDistance = new GraphsPath();
            TraveledDistance.Add(currentPosition);
        }
    }
    class ProbabilityChoice
    {
        public Vertex vertex;
        public double maxVal;
        public double absolut;
        public double minVal;
    }

    class AntColony : IAlgorithm
    {
        private Graph Graph;
        readonly long TimeConstrain;
        public GraphsPath Result { get; set; }
        public double bestScore { get; set; }

        private readonly double NumberOfIterations = 10;           //t
        private readonly double NumberOfAntsPerVertex = 10;       //m
        private readonly double EvaporationCoefficient = 0.9;     //ro
        private readonly double IntensityDerivative = 0.5;        //delta_tal(t, t+1)
        private readonly double InitialPheromonValue = 0.00000001; //tal(0)
        private readonly double TrailImportance = 1;               //alpha
        private readonly double VisibilityImportance = 1;          //beta

        private List<Ant> Ants; 

        public AntColony(Graph graph, long timeConstrain)
        {
            this.Graph = graph;
            this.TimeConstrain = timeConstrain;
        }

        public GraphsPath Performe()
        {
            SetInitialPheromonValue();
            for (var t = 0; t < NumberOfIterations; t++)
            {
                PlaceingAnts();
                var movingAnts = Ants.ToList();
                while (movingAnts.Count != 0)
                {
                    foreach (var ant in movingAnts)
                    {
                        Vertex nextVertex;
                        if ((nextVertex = NextVertexForAnt(ant)) == null)
                        {
                            movingAnts.Remove(ant);
                            continue;
                        }
                        ant.TraveledDistance.Add(nextVertex);
                        UpdatePheromonOnEdge(ant.CurrentPosition.GetEdgeTo(nextVertex));
                        ant.CurrentPosition = nextVertex;

                        if (ant.TraveledDistance.Score > this.bestScore)
                            this.Result = ant.TraveledDistance;
                    }
                }
            }
            return this.Result;
        }

        private Vertex NextVertexForAnt(Ant ant)
        {
            var vertexToCheck = ant.CurrentPosition.ConnectedVertices.Except(ant.TraveledDistance.VerticesSequence);
            if (vertexToCheck.Count() == 0)
                return null;

            var edgesToCheck = new List<Edge>();
            foreach (var v in vertexToCheck)
                edgesToCheck.Add(v.GetEdgeTo(ant.CurrentPosition));

            var probabilityList = TransitionProbabilityFromVertex(ant.CurrentPosition, vertexToCheck, edgesToCheck);
            Random rand = new Random();
            var choice = rand.NextDouble() % probabilityList.Sum(e => e.absolut);
            return probabilityList.Find(c => choice <= c.maxVal && choice > c.minVal).vertex;
        }

        private List<ProbabilityChoice> TransitionProbabilityFromVertex(Vertex fromVertex, IEnumerable<Vertex> vertexToCheck, IEnumerable<Edge> edgesToCheck)
        {
            var result = new List<ProbabilityChoice>();
            double currentValue = 0;
            foreach (var vertex in vertexToCheck)
            {
                var edge = fromVertex.GetEdgeTo(vertex);
                var numerator = edge.PheromonIntensity * TrailImportance * Visibility(edge, vertex) * VisibilityImportance;
                var denumeratot = edgesToCheck.Sum(e => e.PheromonIntensity * TrailImportance * Visibility(e, e.GetVertexOppositTo(fromVertex)) * VisibilityImportance);
                var probability = numerator / denumeratot;
                result.Add(new ProbabilityChoice()
                {
                    minVal = currentValue,
                    absolut = probability,
                    maxVal = currentValue + probability,
                    vertex = vertex
                });
                currentValue += probability;
            }
            return result;
        }

        private double Visibility(Edge edge, Vertex targetVertex)
        {
            return targetVertex.Score / edge.Duration;
        }

        private void UpdatePheromonOnEdge(Edge e)
        {
            e.PheromonIntensity = EvaporationCoefficient*e.PheromonIntensity + IntensityDerivative;
        }

        private void PlaceingAnts()
        {
            Ants = new List<Ant>();
            foreach (var vertex in Graph.Vertices)
            {
                for (var i = 0; i < NumberOfAntsPerVertex; i++)
                {
                    Ants.Add(new Ant(vertex));
                }
            }
        }
        private void SetInitialPheromonValue()
        {
            foreach (var edge in Graph.Edges)
            {
                edge.PheromonIntensity = InitialPheromonValue;
            }
        }
    }
}
