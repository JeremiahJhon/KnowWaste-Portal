using kNowaste.Helper;
using KnowWaste.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Editor;
using System.Web.UI.WebControls;
using UCOnline.Data;
using UCOnline.Models;

namespace Knowwaste.Areas.New.Controllers
{
    public class WasteDataController : Controller
    {
        // GET: New/WasteData
        public ActionResult Index()
        {
            int countryID = Utility.GetCookieInt(Request, "CountryID");
            int subregionID = Utility.GetCookieInt(Request, "SubRegionID");
            int year = Utility.GetCookieInt(Request, "Year");

            WasteData model = new WasteData(subregionID, countryID, year);
            return View(model);
        }

        [HttpGet]
        public JsonResult getWasteChart1(int subregionID, int countryID, int year, string type, int categoryID)
        {
            KnowWasteEntities db = new KnowWasteEntities();

            var rawList = (from a in db.countrywastestreams
                           join b in db.countries on a.Country_ID equals b.ID.ToString()
                           join c in db.wastecategories on a.Wastecategory_ID equals c.ID.ToString()
                           where a.Deleted == false
                                 && (c.ID == 1 || c.ID == 14)
                           select new { a, b, c }).ToList();

            if (!String.IsNullOrWhiteSpace(type))
            {
                
            }

            if(categoryID > 0)
            {
                rawList = rawList.Where(p => p.c.ID == categoryID).ToList();
            }

            List<ViewModels.Data> DataList = rawList.Select(x => new ViewModels.Data
            {
                ID = x.a.ID,
                CountryID = x.b.ID,
                Country = x.b.Name,

                CityID = 0,
                City = "",

                WasteTypeID = x.c.ID,
                WasteType = x.c.Name,

                Year = string.IsNullOrWhiteSpace(x.a.Year) ? 0 : Convert.ToInt32(x.a.Year),

                TotalGenerated = x.a.Totalgenerated,
                Hazardous = x.a.Hazardous,
                TotalCollected = x.a.Totalcollected,
                Recycled = x.a.Recycled,
                Recovered = x.a.Recovered,
                Disposal = x.a.Disposal,
                Treatment = x.a.Treatment,
                Reuse = x.a.Reuse,
                Sludge = x.a.Sludge,
                Reference = x.a.reference,
            }).ToList();

            var data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,

                            Generated = g.Sum(x => x.TotalGenerated ?? 0),
                            Hazardous = g.Sum(x => x.Hazardous ?? 0),
                            Collected = g.Sum(x => x.TotalCollected ?? 0),
                            Recycled = g.Sum(x => x.Recycled ?? 0),
                            Recovered = g.Sum(x => x.Recovered ?? 0),
                            Disposal = g.Sum(x => x.Disposal ?? 0),
                            Treatment = g.Sum(x => x.Treatment ?? 0),
                            Reuse = g.Sum(x => x.Reuse ?? 0),
                            Sludge = g.Sum(x => x.Sludge ?? 0)
                        })
                        .ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getWasteChart2(int subregionID, int countryID, int year, string type)
        {
            KnowWasteEntities db = new KnowWasteEntities();

            var rawList = (from a in db.countrywastestreams
                           join b in db.countries on a.Country_ID equals b.ID.ToString()
                           join c in db.wastecategories on a.Wastecategory_ID equals c.ID.ToString()
                           where a.Deleted == false
                                 && (c.ID == 1 || c.ID == 14)
                           select new { a, b, c }).ToList();

            List<ViewModels.Data> DataList = rawList.Select(x => new ViewModels.Data
            {
                ID = x.a.ID,
                CountryID = x.b.ID,
                Country = x.b.Name,

                CityID = 0,
                City = "",

                WasteTypeID = x.c.ID,
                WasteType = x.c.Name,

                Year = string.IsNullOrWhiteSpace(x.a.Year) ? 0 : Convert.ToInt32(x.a.Year),

                TotalGenerated = x.a.Totalgenerated,
                Hazardous = x.a.Hazardous,
                TotalCollected = x.a.Totalcollected,
                Recycled = x.a.Recycled,
                Recovered = x.a.Recovered,
                Disposal = x.a.Disposal,
                Treatment = x.a.Treatment,
                Reuse = x.a.Reuse,
                Sludge = x.a.Sludge,
                Reference = x.a.reference,
            }).ToList();

            List<ViewModels.WasteData> data = new List<ViewModels.WasteData>();

            switch (type)
            {
                case "Generated":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Generated = g.Sum(x => x.TotalGenerated ?? 0)
                        })
                        .ToList();
                    break;
                case "Hazardous":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Hazardous = g.Sum(x => x.Hazardous ?? 0)
                        })
                        .ToList();
                    break;
                case "Collected":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Collected = g.Sum(x => x.TotalCollected ?? 0)
                        })
                        .ToList();
                    break;
                case "Recycled":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Recycled = g.Sum(x => x.Recycled ?? 0)
                        })
                        .ToList();
                    break;
                case "Recovered":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Recovered = g.Sum(x => x.Recovered ?? 0)
                        })
                        .ToList();
                    break;
                case "Disposal":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Disposal = g.Sum(x => x.Disposal ?? 0)
                        })
                        .ToList();
                    break;
                case "Treatment":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Treatment = g.Sum(x => x.Treatment ?? 0)
                        })
                        .ToList();
                    break;
                case "Reuse":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Reuse = g.Sum(x => x.Reuse ?? 0)
                        })
                        .ToList();
                    break;
                case "Sludge":
                    data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,
                            Sludge = g.Sum(x => x.Sludge ?? 0)
                        })
                        .ToList();
                    break;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}