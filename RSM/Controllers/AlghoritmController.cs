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
                var alg = new RepeatedNearestNeighbor(graph, 150);
                var result = alg.Performe();

            }
            return Json("");
        }

    }
}