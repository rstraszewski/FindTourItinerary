using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAlgorithmsRSM
{
    class AntColony : IAlgorithm
    {
        private Graph _graph;
        readonly long _timeConstrain;
        private GraphsPath _result;

        private readonly double _numberOfAnts = 1000;
        private readonly double _evaporationCoefficient = 0.02;
        private readonly double _intensityDerivative = 0.1;


        public AntColony(Graph graph, long timeConstrain)
        {
            _graph = graph;
            _timeConstrain = timeConstrain;
            _result = new GraphsPath();
        }

        public GraphsPath Performe()
        {
            throw new NotImplementedException();
        }

        private void UpdatePheromonOnEdge(Edge e)
        {
            e.PheromonIntensity = _evaporationCoefficient*e.PheromonIntensity + _intensityDerivative;
        }
    }
}
