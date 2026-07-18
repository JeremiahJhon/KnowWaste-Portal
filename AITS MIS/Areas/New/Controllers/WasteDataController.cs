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
using ViewModels;

namespace Knowwaste.Areas.New.Controllers
{
    public class WasteDataController : Controller
    {
        // GET: New/WasteData

        public ActionResult Index(int subregionID = 0, int countryID = 0, int year = 0)
        {
            // Default to North-Eastern Asia on first load
            if (subregionID == 0)
            {
                subregionID = 1;
            }
            KnowWaste.Models.WasteData model = new KnowWaste.Models.WasteData(subregionID, countryID, year);
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
                                 //&& (c.ID == 1 || c.ID == 14 || c.ID == 4)
                                 && c.Deleted == 0
                           select new { a, b, c }).ToList();

            if (!String.IsNullOrWhiteSpace(type))
            {

            }

            if (categoryID > 0)
            {
                rawList = rawList.Where(p => p.c.ID == categoryID).ToList();
            }

            if (subregionID > 0)
            {
                rawList = rawList.Where(p => p.b.SubRegion_ID == subregionID.ToString()).ToList();
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

            if (countryID > 0)
            {
                DataList = DataList.Where(p => p.CountryID == countryID).ToList();
            }

            if (year > 0)
            {
                DataList = DataList.Where(p => p.Year == year).ToList();
            }

            var data = DataList
                        .GroupBy(x => new { x.WasteTypeID, x.WasteType })
                        .Select(g => new ViewModels.WasteData
                        {
                            WasteCategory = g.Key.WasteType,

                            Generated = g.All(x => x.TotalGenerated == null) ? (decimal?)null : g.Sum(x => x.TotalGenerated ?? 0),
                            Hazardous = g.All(x => x.Hazardous == null) ? (decimal?)null : g.Sum(x => x.Hazardous ?? 0),
                            Collected = g.All(x => x.TotalCollected == null) ? (decimal?)null : g.Sum(x => x.TotalCollected ?? 0),
                            Recycled = g.All(x => x.Recycled == null) ? (decimal?)null : g.Sum(x => x.Recycled ?? 0),
                            Recovered = g.All(x => x.Recovered == null) ? (decimal?)null : g.Sum(x => x.Recovered ?? 0),
                            Disposal = g.All(x => x.Disposal == null) ? (decimal?)null : g.Sum(x => x.Disposal ?? 0),
                            Treatment = g.All(x => x.Treatment == null) ? (decimal?)null : g.Sum(x => x.Treatment ?? 0),
                            Reuse = g.All(x => x.Reuse == null) ? (decimal?)null : g.Sum(x => x.Reuse ?? 0),
                            Sludge = g.All(x => x.Sludge == null) ? (decimal?)null : g.Sum(x => x.Sludge ?? 0)
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
                           where a.Deleted == false && c.Deleted == 0
                           select new { a, b, c }).ToList();

            if (subregionID > 0)
            {
                rawList = rawList.Where(p => p.b.SubRegion_ID == subregionID.ToString()).ToList();
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

            if (countryID > 0)
            {
                DataList = DataList.Where(p => p.CountryID == countryID).ToList();
            }

            if (year > 0)
            {
                DataList = DataList.Where(p => p.Year == year).ToList();
            }

            // Always return every top-level waste category, even ones with no
            // matching rows for the current filter, so the chart legend stays
            // constant across filter changes instead of dropping empty series.
            var allCategories = db.wastecategories
                .Where(p => p.Deleted == 0 && p.Parent == 0)
                .Select(c => new { c.ID, c.Name })
                .ToList();

            List<ViewModels.WasteData> data = allCategories.Select(cat =>
            {
                List<ViewModels.Data> categoryRows = DataList.Where(x => x.WasteTypeID == cat.ID).ToList();
                var wd = new ViewModels.WasteData { WasteCategory = cat.Name };

                switch (type)
                {
                    case "Generated":
                        wd.Generated = SumOrNull(categoryRows, x => x.TotalGenerated);
                        break;
                    case "Hazardous":
                        wd.Hazardous = SumOrNull(categoryRows, x => x.Hazardous);
                        break;
                    case "Collected":
                        wd.Collected = SumOrNull(categoryRows, x => x.TotalCollected);
                        break;
                    case "Recycled":
                        wd.Recycled = SumOrNull(categoryRows, x => x.Recycled);
                        break;
                    case "Recovered":
                        wd.Recovered = SumOrNull(categoryRows, x => x.Recovered);
                        break;
                    case "Disposal":
                        wd.Disposal = SumOrNull(categoryRows, x => x.Disposal);
                        break;
                    case "Treatment":
                        wd.Treatment = SumOrNull(categoryRows, x => x.Treatment);
                        break;
                    case "Reuse":
                        wd.Reuse = SumOrNull(categoryRows, x => x.Reuse);
                        break;
                    case "Sludge":
                        wd.Sludge = SumOrNull(categoryRows, x => x.Sludge);
                        break;
                }

                return wd;
            }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // Returns null when no row has a value for this field (true "no data"),
        // otherwise sums the values, treating any individual null as 0.
        private static decimal? SumOrNull(List<ViewModels.Data> rows, Func<ViewModels.Data, decimal?> selector)
        {
            if (rows.Count == 0 || !rows.Any(x => selector(x) != null))
            {
                return null;
            }
            return rows.Sum(x => selector(x) ?? 0);
        }
    }
}