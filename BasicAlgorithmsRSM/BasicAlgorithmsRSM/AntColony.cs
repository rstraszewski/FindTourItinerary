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
        public bool moving;

        public Ant(Vertex currentPosition)
        {
            CurrentPosition = currentPosition;
            TraveledDistance = new GraphsPath();
            TraveledDistance.Add(currentPosition);
            moving = true;
        }
    }
    class ProbabilityChoice
    {
        public Vertex vertex;
        public double maxVal;
        public double absolut;
        public double minVal;
    }
    public class AntColonyParameters
    {
        public double TimeConstrain { get; set; }
        public double NumberOfIterations { get; set; }
        public double NumberOfAntsPerVertex { get; set; }
        public double EvaporationCoefficient { get; set; }
        public double IntensityDerivative { get; set; }
        public double InitialPheromonValue { get; set; }
        public double TrailImportance { get; set; }
        public double VisibilityImportance { get; set; }

    }
    public class AntColony : IAlgorithm
    {
        private Graph Graph;
        public GraphsPath Result { get; set; }
        public double bestScore { get; set; }

        private double TimeConstrain;
        private double NumberOfIterations;// = 100;           //t
        private double NumberOfAntsPerVertex;// = 10;       //m
        private double EvaporationCoefficient;// = 0.9;     //ro
        private double IntensityDerivative;// = 0.5;        //delta_tal(t, t+1)
        private double InitialPheromonValue;// = 0.00000001; //tal(0)
        private double TrailImportance;// = 1;               //alpha
        private double VisibilityImportance;// = 1;          //beta

        private List<Ant> Ants;

        public AntColony(Graph graph, AntColonyParameters parameters)
        {
            this.Graph = graph;
            this.TimeConstrain = parameters.TimeConstrain;
            this.NumberOfIterations = parameters.NumberOfIterations != 0 ? parameters.NumberOfIterations : 100;
            this.NumberOfAntsPerVertex = parameters.NumberOfAntsPerVertex != 0 ? parameters.NumberOfAntsPerVertex : 10;
            this.EvaporationCoefficient = parameters.EvaporationCoefficient != 0 ? parameters.EvaporationCoefficient : 0.9;
            this.IntensityDerivative = parameters.IntensityDerivative != 0 ? parameters.IntensityDerivative : 0.5;
            this.InitialPheromonValue = parameters.InitialPheromonValue != 0 ? parameters.InitialPheromonValue : 0.000001;
            this.TrailImportance = parameters.TrailImportance != 0 ? parameters.TrailImportance : 1;
            this.VisibilityImportance = parameters.VisibilityImportance != 0 ? parameters.VisibilityImportance : 1;
        }

        public GraphsPath Performe()
        {
            SetInitialPheromonValue();
            for (var t = 0; t < NumberOfIterations; t++)
            {
                PlaceingAnts();
                var movingAnts = Ants.Count;
                while (movingAnts != 0)
                {
                    foreach (var ant in Ants.Where(a=>a.moving))
                    {
                        Vertex nextVertex;
                        if ((nextVertex = NextVertexForAnt(ant)) == null)
                        {
                            ant.moving = false;
                            movingAnts--;
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
            var vertexToCheck = ant.CurrentPosition.ConnectedVertices
                .Except(ant.TraveledDistance.VerticesSequence)
                .Where(e => ant.TraveledDistance.Duration + ant.CurrentPosition.GetEdgeTo(e).Duration <= TimeConstrain);
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
            return targetVertex.Score;// / edge.Duration;
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
