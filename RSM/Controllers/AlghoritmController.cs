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
                    var saResult = new SimulatedAnnealingResult()
                    {
                        Parameters = parameters,
                        DataSet = saTask.DataSet,
                        Id = saTask.Id
                    };

                    for (var i = 0; i < saTask.HomManyTimes; i++)
                    {
                        var saAlg = new SimulatedAnnealing(graph, parameters);
                        var resultSa = saAlg.Performe();
                        var ids = resultSa.VerticesSequence.Select(v => v.Id);
                        var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();
                        saResult.Paths.Add(locations);
                        saResult.Scores.Add(resultSa.Score);
                    }
                    result.SimulatedAnnealingResults.Add(saResult);
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