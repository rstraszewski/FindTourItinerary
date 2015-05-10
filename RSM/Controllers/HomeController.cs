using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Bing;
using Bing.Spatial;
using EntityFramework.BulkInsert.Extensions;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RSM.Models;
using RSM.Models.BingMapsRESTService.Common.JSON;
using RSM.Models.Foursquare;
using Response = RSM.Models.BingMapsRESTService.Common.JSON.Response;

namespace RSM.Controllers
{
    public class HomeController : Controller
    {

        public class RsmDbContext : DbContext
        {
            public DbSet<DataSet> DataSets { get; set; }
            public DbSet<Location> Locations { get; set; }
            public DbSet<LocationRoute> LocationRoutes { get; set; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public class DataSet
        {
            public long Id { get; set; }
            public List<Location> Locations { get; set; }
            public List<LocationRoute> Routes { get; set; }
        }

        public class Location
        {
            public long Id { get; set; }
            public string ForsquareId { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Name { get; set; }
            public float Rate { get; set; }
            public string Address { get; set; }
            public string Category { get; set; }

            public string LocationAsString()
            {
                return Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat) + "," + Longitude.ToString(CultureInfo.InvariantCulture.NumberFormat);
            }
        }

        public class LocationRoute
        {
            public long Id { get; set; }
            public Location LocA { get; set; }
            public Location LocB { get; set; }
           public double DurationInSeconds { get; set; }

            public string DurationAsString
            {
                get
                {
                    var t = TimeSpan.FromSeconds(DurationInSeconds);
                    return string.Format("{0:D2} hours and {1:D2} minutes",
                            t.Hours,
                            t.Minutes);
                }
            }

            public double DurationInHours
            {
                get { return DurationInSeconds/3600; }
            }

            public double Distance { get; set; }

        }

        public async Task<ActionResult> SaveDataSet(string locations)
        {
            var locationList = JsonConvert.DeserializeObject<List<Location>>(locations);
            var routes = await GetRoutesBing(locationList);

            var dataSet = new DataSet() {Locations = locationList, Routes = routes};

            using (var db = new RsmDbContext())
            {
                //db.DataSets.Add(dataSet);
                db.BulkInsert(new List<DataSet>(){dataSet});
                db.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> FindLocations(string query, string near, int offset = 0)
        {
            var client = new RestClient("https://api.foursquare.com");
            var request = new RestRequest("v2/venues/explore");
            request.AddParameter("v", "20150509");
            request.AddParameter("client_secret", "I1RXVY3TW24ZP2TXBYQPSS1Z4DC3N4XZKFXNXIAAEQVRN3GA");
            request.AddParameter("client_id", "WNMRUNEAENC5QS1C2FHVBZTO3TSY2TLOCQFSMOKCHHC3TO12");
            request.AddParameter("client_id", "WNMRUNEAENC5QS1C2FHVBZTO3TSY2TLOCQFSMOKCHHC3TO12");
            request.AddParameter("near", near);
            request.AddParameter("query", query);
            request.AddParameter("offset", offset);

            var response = await client.ExecuteGetTaskAsync(request);
            var fResponse = JsonConvert.DeserializeObject<FoursquareResponse>(response.Content);
            var locations = fResponse.response.groups.First().items.Select(item => new Location()
            {
                ForsquareId = item.venue.id,
                Latitude = item.venue.location.lat,
                Longitude = item.venue.location.lng,
                Address = string.Join(", ", item.venue.location.formattedAddress),
                Name = item.venue.name,
                Rate = item.venue.rating,
                Category = item.venue.categories.First().name
            });

            return Json(new { locations }, JsonRequestBehavior.AllowGet);
        }

        public async Task<List<LocationRoute>> GetRoutesBing(IEnumerable<Location> locations)
        {
            var client = new RestClient("http://dev.virtualearth.net");
            var request = new RestRequest("REST/v1/Routes/Walking");
            var ser = new DataContractJsonSerializer(typeof(Response));
            var taskList = new List<Task<IRestResponse>>();
            var routes = new List<LocationRoute>();

            var locationList = locations as IList<Location> ?? locations.ToList();
            for (int i = 0; i < locationList.Count; i++)
            {
                var location1 = locationList[i];
                for (int j = 0; j < locationList.Count; j++)
                {
                    var location2 = locationList[j];
                    if (i == j) continue;
                    if (routes.Any(route =>
                                (route.LocA.Equals(location1) && route.LocB.Equals(location2)) 
                                || (route.LocB.Equals(location1) && route.LocA.Equals(location2)))) continue;

                    routes.Add(new LocationRoute()
                    {
                        LocA = location1,
                        LocB = location2
                    });
                }
            }

            for (int i = 0; i < routes.Count; i++)
            {
                var route = routes[i];
                request.AddParameter("key", "AuM7che94B5gbSaSIvcb7kXrr_tW7ZMq81q_rfaZibeZtXakscM4u-WP9OB7K58V");
                request.AddParameter("travelMode", "Walking");
                request.AddParameter("waypoint.1", route.LocA.LocationAsString());
                request.AddParameter("waypoint.2", route.LocB.LocationAsString());

                var response = client.ExecuteGetTaskAsync(request);
                taskList.Add(response);

                request.Parameters.Clear();
            }


            Task.WaitAll(taskList.ToArray());

            var results = taskList.Select(t => t.Result.Content).ToList();

            for (var i = 0; i<results.Count();i++)
            {
                var result = results[i];
                using (var str = GenerateStreamFromString(result))
                {
                    var obj = ser.ReadObject(str) as Response;

                    var routeResponse = (Route)obj.ResourceSets.FirstOrDefault().Resources.FirstOrDefault();

                    routes[i].Distance = routeResponse.TravelDistance;
                    routes[i].DurationInSeconds = routeResponse.TravelDuration;
                }
            }


            return routes;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}