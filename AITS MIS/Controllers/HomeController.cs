using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using UCOnline.Data;
using UCOnline.Models;
using Web.Framework.Controllers;
using Web.Framework.Server;
using kNowaste.Helper;

namespace UCOnline.Controllers
{
    public class HomeController : BaseController
    {
        KnowWasteEntities context = new KnowWasteEntities();
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

            var JoinResult = from a in context.blogs.Where(x => x.Blogscategory_ID == 1 && x.Deleted == 0).OrderByDescending(x => x.ID).Take(10)
                             join b in context.countries.Where(x => x.SubRegion_ID != null && x.SubRegion_ID != "" && x.Deleted == 0)
                             on a.Country_ID equals b.ID
                             select new
                             {
                                    a.ID,
                                    a.Title,
                                    a.Description,
                                    a.Detail,
                                    a.Author,
                                    a.Thumbnail,
                                    Country = b.Name,
                                    a.ResultsArchieved,
                                    a.ChallengesLessonLearned,
                                    a.Replicability,
                                    a.Sources,
                                    a.Company,
                                    a.Email,
                                    a.Phone,
                                    a.Initiative,
                                    a.Photo,
                             };

            ViewBag.Blogs = Utility.LinqToDataTable(JoinResult);

            return View();
        }
    }
}