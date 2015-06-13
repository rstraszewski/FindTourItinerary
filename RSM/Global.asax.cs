using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BasicAlgorithmsRSM;
using RSM.Controllers;
using RSM.Entities;
using RSM.Models;

namespace RSM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterMappings();
            using (var context = new RsmDbContext())
            {
                context.Database.Initialize(false);
            }
        }

        private void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<SimulatedAnnealingTask, SimulatedAnnealingParameters>();
            AutoMapper.Mapper.CreateMap<AntColonyTask, AntColonyParameters>();
            AutoMapper.Mapper.CreateMap<RepeatedNearestNeighborTask, RepeatedNearestNeighborParameters>();
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;


                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            
        }
    }
}
