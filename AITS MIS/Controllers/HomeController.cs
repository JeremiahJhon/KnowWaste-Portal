using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;
using Web.Framework.Server;

namespace UCOnline.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : base(new ModelHome())
        {
        }

        public override ActionResult Index()
        {
            ViewBag.Title = "kNOw.Waste Management Platform";
            ViewBag.ThematicmenuHome = ((ModelHome)Model).ShowThematicMenuHome();
            ViewBag.Seminar = new ModelSeminars().GetData(1);                                
            ViewBag.Conferences = new ModelConferences().GetData(1);
            ViewBag.Rosters = new ModelExperts().GetData();
            //ViewBag.News = new ModelNews().GetData(2);
            ViewBag.Tweets = new ModelNews().Tweets();
            ViewBag.Events = new ModelEvents().GetData(4);
            //ViewBag.Blogs = new ModelBlogs().GetData();
            ViewBag.Document = new ModelReports().ShowThumbnail(6);

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

            foreach (DataRow row in data.Rows)
                if (row["id"].ToString() == countryid)
                {
                    handle = false;
                    break;
                }

            if (handle && data.Rows.Count != 0 && subregionid != null)
                countryid = data.Rows[0]["id"].ToString();

            var year = Request.QueryString["year"];

            var wstream = new ModelWasteStream();

            if (year == null)
                year = "0";

                //wstream.GetYear(countryid, ref year);

            ViewBag.Year = wstream.GetYear(countryid, ref year);

            var model = new ModelData();

            model.Year = year;
            model.CountryID = countryid;

            var country = new ModelCountry();
            country.SubRegionID = subregionid;
            country.CountyID = countryid;

            ViewBag.Country = country.GetCombobox();

            var subregion = new ModelSubRegion();
            subregion.RegionID = regionid;
            subregion.SubRegionID = subregionid;

            ViewBag.SubRegion = subregion.GetCombobox();

            var region = new ModelRegion();
            region.RegionID = regionid;

            ViewBag.Region = region.GetCombobox();

            return View();
        }
    }
}