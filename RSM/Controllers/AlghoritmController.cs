using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                ViewBag.DataSet.AddRange(dbContext.DataSets.Select(ds => new DataSetViewModel(){Id = ds.Id, Name = ds.Name}));
            }
            return View();
        }

        public ActionResult Run(long dataSetId, bool isAntChecked, bool isNnChecked, bool isSaChecked)
        {
            using (var dbContext = new RsmDbContext())
            {
                var dataSet = dbContext.DataSets.Find(dataSetId);
                var graph = new Graph(dataSet);
                if (isNnChecked)
                {
                    var alg = new RepeatedNearestNeighbor(graph, 15000);
                    var result = alg.Performe();
                    var ids = result.VerticesSequence.Select(v => v.Id);
                    var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();
                    return Json(new { locations });
                }

                if (isAntChecked)
                {
                    var alg = new AntColony(graph, 15000);
                    var result = alg.Performe();
                    var ids = result.VerticesSequence.Select(v => v.Id);
                    var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();
                    return Json(new { locations });
                }

                if (isSaChecked)
                {
                    var saAlg = new SimulatedAnnealing(graph, 15000);
                    var resultSa = saAlg.Performe(30000);
                    var ids = resultSa.VerticesSequence.Select(v => v.Id);
                    var locations = dbContext.Locations.Where(loc => ids.Contains(loc.Id)).ToList();
                    return Json(new {locations});
                }
            }
            return Json("");
        }

    }
}