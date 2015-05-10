using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSM.Models;

namespace RSM.Controllers
{
    public class AlghoritmController : Controller
    {
        // GET: Alghoritm
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RunAlghoritm(long dataSetId)
        {
            using (var dbContext = new RsmDbContext())
            {
                var dataSet = dbContext.DataSets.Find(dataSetId);
                
            }

            return Json("");
        }
    }
}