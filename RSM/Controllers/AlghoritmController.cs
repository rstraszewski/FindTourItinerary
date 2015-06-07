using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BasicAlgorithmsRSM;
using RSM.Entities;
using RSM.Models;

namespace RSM.Controllers
{
    public class DataSetViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

    }
    public class AlghoritmController : Controller
    {

        // GET: Alghoritm
        public ActionResult Index()
        {
            ViewBag.DataSet = new List<DataSetViewModel>();
            using (var dbContext = new RsmDbContext())
            {
                ViewBag.DataSet.AddRange(dbContext.DataSets.Select(ds => new DataSetViewModel() { Id = ds.Id, Name = ds.Name }));
            }
            return View();
        }

        public class SimulatedAnnealingTask
        {
            public DataSetViewModel DataSet { get; set; }
            public int NumberOfIteration { get; set; }
            public float TemperatureAlpha { get; set; }
            public double TemperatureMax { get; set; }
            public double TimeConstrain { get; set; }
            public int Id { get; set; }
        }
        public class AntColonyTask
        {
            public DataSetViewModel DataSet { get; set; }
            public double TimeConstrain { get; set; }
            public double NumberOfIterations { get; set; }
            public double NumberOfAntsPerVertex { get; set; }
            public double EvaporationCoefficient { get; set; }
            public double IntensityDerivative { get; set; }
            public double InitialPheromonValue { get; set; }
            public double TrailImportance { get; set; }
            public double VisibilityImportance { get; set; }
            public int Id { get; set; }


        }

        public class RepeatedNearestNeighborTask
        {
            public DataSetViewModel DataSet { get; set; }
            public double TimeConstrain { get; set; }
            public int Id { get; set; }


        }

        public class SimulatedAnnealingResult
        {
            public SimulatedAnnealingParameters Parameters { get; set; }
            public List<Location> Path { get; set; }
            public double Score { get; set; }
            public DataSetViewModel DataSet { get; set; }
            public int Id { get; set; }

        }

        public class AntColonyResult
        {
            public AntColonyParameters Parameters { get; set; }
            public List<Location> Path { get; set; }
            public double Score { get; set; }
            public DataSetViewModel DataSet { get; set; }
            public int Id { get; set; }

        }

        public class RepeatedNearestNeighborResult
        {
            public RepeatedNearestNeighborParameters Parameters { get; set; }
            public List<Location> Path { get; set; }
            public double Score { get; set; }
            public DataSetViewModel DataSet { get; set; }
            public int Id { get; set; }

        }

        public class Result
        {
            public List<SimulatedAnnealingResult> SimulatedAnnealingResults { get; set; }
            public List<AntColonyResult> AntColonyResults { get; set; }
            public List<RepeatedNearestNeighborResult> RepeatedNearestNeighborResults { get; set; }

            public double AverageSaScore
            {
                get { return SimulatedAnnealingResults.Count > 0 ? SimulatedAnnealingResults.Sum(e => e.Score) / SimulatedAnnealingResults.Count : 0; }
            }

            public double AverageNnScore
            {
                get { return RepeatedNearestNeighborResults.Count > 0 ? RepeatedNearestNeighborResults.Sum(e => e.Score) / RepeatedNearestNeighborResults.Count : 0; }
            }

            public double AverageAntScore
            {
                get { return AntColonyResults.Count > 0 ? AntColonyResults.Sum(e => e.Score) / AntColonyResults.Count : 0; }
                
            }
            public Result()
            {
                SimulatedAnnealingResults = new List<SimulatedAnnealingResult>();
                AntColonyResults = new List<AntColonyResult>();
                RepeatedNearestNeighborResults = new List<RepeatedNearestNeighborResult>();
            }
        }

        public ActionResult Run(List<SimulatedAnnealingTask> saTasks, List<RepeatedNearestNeighborTask> nnTasks, List<AntColonyTask> antTasks)
        {
            var result = new Result();
            using (var dbContext = new RsmDbContext())
            {
                if(saTasks!=null)
                foreach (var saTask in saTasks)
                {
                    var parameters = Mapper.Map<SimulatedAnnealingParameters>(saTask);
                    var dataSet = dbContext.DataSets.Find(saTask.DataSet.Id);
                    var graph = new Graph(dataSet);
                    var saAlg = new SimulatedAnnealing(graph, parameters);
                    var resultSa = saAlg.Performe();
                    var ids = resultSa.VerticesSequence.Select(v => v.Id);
                    var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();

                    result.SimulatedAnnealingResults.Add(new SimulatedAnnealingResult()
                    {
                        Parameters = parameters,
                        DataSet = saTask.DataSet,
                        Path = locations,
                        Score = resultSa.Score,
                        Id = saTask.Id
                    });
                }
                if (nnTasks != null)
                foreach (var nnTask in nnTasks)
                {
                    var parameters = Mapper.Map<RepeatedNearestNeighborParameters>(nnTask);
                    var dataSet = dbContext.DataSets.Find(nnTask.DataSet.Id);
                    var graph = new Graph(dataSet);
                    var nnAlg = new RepeatedNearestNeighbor(graph, parameters);
                    var resultSa = nnAlg.Performe();
                    var ids = resultSa.VerticesSequence.Select(v => v.Id);
                    var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();

                    result.RepeatedNearestNeighborResults.Add(new RepeatedNearestNeighborResult()
                    {
                        Parameters = parameters,
                        DataSet = nnTask.DataSet,
                        Path = locations,
                        Score = resultSa.Score,
                        Id = nnTask.Id

                    });
                }
                if (antTasks != null)
                foreach (var antTask in antTasks)
                {
                    var parameters = Mapper.Map<AntColonyParameters>(antTask);
                    var dataSet = dbContext.DataSets.Find(antTask.DataSet.Id);
                    var graph = new Graph(dataSet);
                    var antAlg = new AntColony(graph, parameters);
                    var resultAnt = antAlg.Performe();
                    var ids = resultAnt.VerticesSequence.Select(v => v.Id);
                    var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();

                    result.AntColonyResults.Add(new AntColonyResult()
                    {
                        Parameters = parameters,
                        DataSet = antTask.DataSet,
                        Path = locations,
                        Score = resultAnt.Score,
                        Id = antTask.Id

                    });
                }

            }

            return Json(result);
        }

       
    }
}