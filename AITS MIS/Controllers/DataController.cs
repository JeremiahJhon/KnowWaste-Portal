using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Entity;
using UCOnline.Models;
using Web.Framework.Controllers;
using Web.Framework.Server;

namespace UCOnline.Controllers
{
    public class DataController :  BaseController
    {
        public DataController() : base(new ModelData())
        {
            ViewBag.Title = "Data and Trends - kNOw.Waste Management Platform";            
        }

        public override ActionResult Index()
        {
            ViewBag.ThematicmenuHome = new ModelHome().ShowThematicMenuHome();
            var regionid = Request.QueryString["regionid"];
            var subregionid = Request.QueryString["subregionid"];
            var countryid = Request.QueryString["countryid"];

            if (regionid == null)
                regionid = "3";

            ServerBase server = new ServerBase("subregion");
            server.SelectFilter("region_id", regionid);

            DataTable data = server.SelectQuery();

            var handle = true;

            foreach (DataRow row in data.Rows)
                if (row["id"].ToString() == subregionid)
                {
                    handle = false;
                    break;
                }

            if (handle && data.Rows.Count != 0)
                subregionid = data.Rows[0]["id"].ToString();

            if (countryid == null && subregionid != null)
                countryid = "2";

            server = new ServerBase("country");
            server.SelectFilter("subregion_id", subregionid);

            data = server.SelectQuery();

            handle = true;

            if (countryid != "0")
            {
                foreach (DataRow row in data.Rows)
                    if (row["id"].ToString() == countryid)
                    {
                        handle = false;
                        break;
                    }

                if (handle && data.Rows.Count != 0 && subregionid != null)
                    countryid = data.Rows[0]["id"].ToString();
            }

            var year = Request.QueryString["year"];

            var wstream = new ModelWasteStream();

            if (year == null)
                wstream.GetYear(countryid, ref year);

            if (subregionid != null)
            {
                if (countryid != null)
                {
                    ViewBag.Year = wstream.GetYear(countryid, ref year);

                    ((ModelData)Model).Year = year;
                    ((ModelData)Model).CountryID = countryid;

                    //List of Countries
                    var country = new ModelCountry();
                    country.SubRegionID = subregionid;
                    country.CountyID = countryid;
                    ViewBag.Country = country.GetCombobox();
                }

                //List Subregions
                var subregion = new ModelSubRegion();
                subregion.RegionID = regionid;
                subregion.SubRegionID = subregionid;
                ViewBag.SubRegion = subregion.GetCombobox();
            }

            //List of Regions
            var region = new ModelRegion();
            region.RegionID = regionid;
            ViewBag.Region = region.GetCombobox();

            return base.Index();
        }

        [HttpGet]
        public override ActionResult Data()
        {
            var regionid = Request.QueryString["regionid"];
            var subregionid = Request.QueryString["subregionid"];
            var countryid = Request.QueryString["countryid"];

            if (regionid == null)
                regionid = "3";

            ServerBase server = new ServerBase("subregion");
            server.SelectFilter("region_id", regionid);

            DataTable data = server.SelectQuery();

            var handle = true;

            foreach (DataRow row in data.Rows)
                if (row["id"].ToString() == subregionid)
                {
                    handle = false;
                    break;
                }

            if (handle && data.Rows.Count != 0)
                subregionid = data.Rows[0]["id"].ToString();

            if (countryid == null && subregionid != null)
                countryid = "2";

            server = new ServerBase("country");
            server.SelectFilter("subregion_id", subregionid);

            data = server.SelectQuery();

            handle = true;

            if (countryid != "0")
            {
                foreach (DataRow row in data.Rows)
                    if (row["id"].ToString() == countryid)
                    {
                        handle = false;
                        break;
                    }

                if (handle && data.Rows.Count != 0 && subregionid != null)
                    countryid = data.Rows[0]["id"].ToString();
            }

            var year = Request.QueryString["year"];

            var wstream = new ModelWasteStream();

            if (year == null)
                wstream.GetYear(countryid, ref year);

            var stream = new ModelWasteStream();
            stream.CountryID = countryid;
            stream.SubRegionID = subregionid;
            stream.Year = year;

            var waste = new List<Waste>();
            waste = stream.GetObject();

            return Json(waste, JsonRequestBehavior.AllowGet);
        }
    }
}