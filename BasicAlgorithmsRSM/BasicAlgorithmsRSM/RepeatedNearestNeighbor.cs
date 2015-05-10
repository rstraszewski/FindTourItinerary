using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    class RepeatedNearestNeighbor : IAlgorithm
    {
        readonly Graph _graph;
        readonly long _timeConstrain;
        private GraphsPath _result;

        public RepeatedNearestNeighbor(Graph graph, long timeConstrain)
        {
            this._graph = graph;
            this._timeConstrain = timeConstrain;
            this._result = new GraphsPath();
        }

        public GraphsPath Performe()
        {
            if (_graph.Vertices != null)
            {
                double bestScore = 0;
                foreach (var vertex in _graph.Vertices)
                {
                    var path = new GraphsPath();
                    path.VerticesSequence.Add(vertex);
                    FindBestPathFrom(path, vertex.Duration);
                    path.Show();
                    if (path.SumScore() > bestScore)
                    {
                        _result = path;
                        bestScore = path.SumScore();
                    }
                    _graph.TurnAllVerticesToNotVisited();
                }
            }
            return _result;
        }

        private void FindBestPathFrom(GraphsPath path, double totalTime)
        {
            Vertex startVertex = path.VerticesSequence.Last();
            startVertex.Visited = true;

            Vertex bestVertex = null;
            double bestScore = 0;
            var bestTime = double.MaxValue;

            foreach (var connectedVertex in startVertex.ConnectedVertices.Where(v => v.Visited == false))
            {
                var edge = _graph.GetEdge(startVertex, connectedVertex);
                var timing = totalTime + connectedVertex.Duration + edge.Duration;
                var score = FactorOfAdvantage(edge, connectedVertex);
                if ( score > bestScore && timing < _timeConstrain)
                {
                    bestScore = score;
                    bestTime = timing;
                    bestVertex = connectedVertex;
                }
            }
            if (bestVertex == null)
            {
                return;
            }
            path.VerticesSequence.Add(bestVertex);
            FindBestPathFrom(path, bestTime);
        }

        private double FactorOfAdvantage(Edge edge, Vertex targetVertex)
        {
            return targetVertex.Score/edge.Duration;
        }
        
    }
}
